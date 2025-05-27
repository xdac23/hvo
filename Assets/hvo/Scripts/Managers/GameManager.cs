using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class GameManager : SingletonManager<GameManager>
{

    private Vector2 m_InitialTouchPosition;
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
        Debug.Log(inputPosition);
    }
    

}