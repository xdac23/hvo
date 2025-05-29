using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using System;

public class HumanoidUnit : Unit
{
    protected Vector2 m_velocity;
    protected Vector3 m_LastPosition;

    public float CurrentSpeed => m_velocity.magnitude;

    void Start()
    {
        m_LastPosition = transform.position;
    }

    protected void Update()
    {
        m_velocity = new Vector2(
            (transform.position.x - m_LastPosition.x),
            (transform.position.y - m_LastPosition.y)
        ) / Time.deltaTime;

        m_LastPosition = transform.position;

        isMoving = m_velocity.magnitude > 0;

        m_Animator.SetFloat("Speed", Mathf.Clamp01(CurrentSpeed));
    }
}

