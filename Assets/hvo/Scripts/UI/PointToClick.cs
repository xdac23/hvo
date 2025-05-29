using System.Linq.Expressions;
using System.Numerics;
using System.Threading;
using UnityEditor.Tilemaps;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PointToClick : MonoBehaviour
{
    [SerializeField] private float m_Duration = 1f;
    [SerializeField] private SpriteRenderer m_SpriteRendered;
    [SerializeField] private AnimationCurve m_ScaleCurve;
    private Vector3 m_InitialScale;

    private float m_Timer;
    private float m_FreqTimer;

    void Start()
    {
        m_InitialScale = transform.localScale;
    }

    void Update()
    {
        m_Timer += Time.deltaTime;
        m_FreqTimer += Time.deltaTime;
        //m_FreqTimer %= 1f; // expensive

        if (m_FreqTimer >= 1f)
        {
            m_FreqTimer = 0;
        }

        float scaleMultiplier = m_ScaleCurve.Evaluate(m_FreqTimer);
        transform.localScale = m_InitialScale * scaleMultiplier;

        if (m_Timer >= m_Duration * 0.9f)
        {
            float fadeProgress = (m_Timer - m_Duration * 0.9f) / (m_Duration * 0.1f);
            m_SpriteRendered.color = new Color(1, 1, 1, 1 - fadeProgress);
        }

        if (m_Timer >= m_Duration)
        {
            Destroy(gameObject);
        }
    }
}