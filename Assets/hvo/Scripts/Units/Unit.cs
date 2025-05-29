using System.Numerics;
using TMPro;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public abstract class Unit : MonoBehaviour
{    
    public bool isMoving;
    public bool isTargeted;

    protected Animator m_Animator;
    protected AIPawn m_AIPawn;
    protected SpriteRenderer m_SprinteRendered;
    protected Material m_OriginalMaterial;
    protected Material m_HighlightMaterial;

    protected void Awake()
    {
        if (TryGetComponent<Animator>(out var animator))
        {
            m_Animator = animator;
        }

        if (TryGetComponent<AIPawn>(out var aiPawn))
        {
            m_AIPawn = aiPawn;
        }

        m_SprinteRendered = GetComponent<SpriteRenderer>();
        m_OriginalMaterial = m_SprinteRendered.material;
        m_HighlightMaterial = Resources.Load<Material>("Materials/Outline");

    }

    public void MoveTo(Vector3 destination)
    {
        var direction = (destination - transform.position).normalized;
        m_SprinteRendered.flipX = direction.x < 0;
        m_AIPawn.SetDestination(destination);
    }

    public void Select()
    {
        Highlight();          
        isTargeted = true;
    }

    public void Deselect()
    {
        UnHighlight();
        isTargeted = false;

    }

    void Highlight()
    {
        m_SprinteRendered.material = m_HighlightMaterial;

    }

    void UnHighlight()
    {
        m_SprinteRendered.material = m_OriginalMaterial;
    }

    

}