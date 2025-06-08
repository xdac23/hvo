using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class GameManager : SingletonManager<GameManager>
{
    [Header("UI")]
    [SerializeField]
    private PointToClick m_pointToClickPrefab;
    [SerializeField] private ActionBar m_actionBar;

    public Unit ActiveUnit;
    private Vector2 m_InitialTouchPosition;
    public Vector2 InputPosition => Input.touchCount > 0 ? Input.GetTouch(0).position : Input.mousePosition;
    public bool IsLeftClickOrTapDown => Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == UnityEngine.TouchPhase.Began);
    public bool IsLeftClickOrTapUp => Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == UnityEngine.TouchPhase.Ended);

    public bool HasActiveUnit => ActiveUnit != null;

    void Start()
    {
        ClearActionBarUI();

    }
    void Update()
    {
        
        if (IsLeftClickOrTapDown)
        {
            m_InitialTouchPosition = InputPosition;
        }

        if (IsLeftClickOrTapUp)
        {

            if (Vector2.Distance(m_InitialTouchPosition, InputPosition) < 5)
            {
                DetectClick(InputPosition);
            }
        }
    }
    void DetectClick(Vector2 inputPosition)
    {
        if (isPointerOverUIElement())
        {
            return;
        }
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(inputPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

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
        ShowUnitActions();
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
        ClearActionBarUI();        
    }

    void DisplayClickEffect(Vector2 worldPoint)
    {

        Instantiate(m_pointToClickPrefab, (Vector3)worldPoint, Quaternion.identity);
    }

    void ShowUnitActions()
    {
        ClearActionBarUI();

        var hardcodedActions = 2;

        for (int i = 0; i < hardcodedActions; i++)
        {
            m_actionBar.RegisterAction();
        }
        m_actionBar.Show();
    }

    void ClearActionBarUI()
    {
        m_actionBar.ClearActions();
        m_actionBar.Hide();
    }

    bool isPointerOverUIElement()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            return EventSystem.current.IsPointerOverGameObject(touch.fingerId);
        }
        else
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
    }

}