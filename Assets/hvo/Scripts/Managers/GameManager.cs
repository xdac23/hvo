using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class GameManager : SingletonManager<GameManager>
{

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
        ActiveUnit.MoveTo(worldPoint);
    }

    void HandleClickOnUnit(Unit unit)
    {
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

}