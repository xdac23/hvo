using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class GameManager : SingletonManager<GameManager>
{
    [Header("UI")]
    [SerializeField]
    private PointToClick m_pointToClickPrefab;

    public Unit ActiveUnit;
    private Vector2 m_InitialTouchPosition;

    public bool HasActiveUnit => ActiveUnit != null;
    void Update()
    {
        // Check for touch the screen or mouse position
        Vector2 inputPosition = Input.touchCount > 0 ? Input.GetTouch(0).position : Input.mousePosition;

        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == UnityEngine.TouchPhase.Ended))
        {
            m_InitialTouchPosition = inputPosition;
        }

        // If the left mouse button is release or a touch ends, call DetectClick
        if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == UnityEngine.TouchPhase.Began))
        {

            if (Vector2.Distance(m_InitialTouchPosition, inputPosition) < 10)
            {
                DetectClick(inputPosition);
            }
        }
    }
    void DetectClick(Vector2 inputPosition)
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(inputPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        Debug.Log(worldPoint);

        if (HasClickOnUnit(hit, out var unit))
        {
            HandleClickOnUnit(unit);
        }
        else
        {
            HandleClickOnGround(worldPoint);
        }
    }

    bool HasClickOnUnit(RaycastHit2D hit, out Unit unit)
    {
        if (hit.collider != null && hit.collider.TryGetComponent<Unit>(out var clickedUnit))
        {
            unit = clickedUnit;
            return true;
        }

        unit = null;
        return false;

    }

    void HandleClickOnGround(Vector2 worldPoint)
    {
        //Debug.Log(worldPoint);

        if (HasActiveUnit && IsHumanoid(ActiveUnit))
        {
            DisplayClickEffect(worldPoint);
            ActiveUnit.MoveTo(worldPoint);
        }


    }

    void HandleClickOnUnit(Unit unit)
    {
        if (HasActiveUnit)
        {
            if (HasClickedOnActiveUnit(unit))
            {
                CancelActiveUnit();
                return;
            }
            
        }
        SelectNewUnit(unit);
    }

    void SelectNewUnit(Unit unit)
    {

        if (ActiveUnit)
        {
            ActiveUnit.Deselect();
        }
        ActiveUnit = unit;
        ActiveUnit.Select();
    }

    bool HasClickedOnActiveUnit(Unit clickedUnit)
    {
        return clickedUnit == ActiveUnit;
    }

    bool IsHumanoid(Unit unit)
    {
        return unit is HumanoidUnit;
    }

    void CancelActiveUnit()
    {
        ActiveUnit.Deselect();
        ActiveUnit = null;

    }

    void DisplayClickEffect(Vector2 worldPoint)
    {

        Instantiate(m_pointToClickPrefab, (Vector3)worldPoint, Quaternion.identity);
    }

}