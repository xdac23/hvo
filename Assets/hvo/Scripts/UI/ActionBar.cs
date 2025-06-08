using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections.Generic;

public class ActionBar : MonoBehaviour
{
    [SerializeField] private Image m_BackgroundImage;
    [SerializeField] private ActionButton m_ActionButtonPrefab;
    private Color m_OriginalBackgroundColor;
    private List<ActionButton> m_ActionButtons = new();
    void Awake()
    {
        m_OriginalBackgroundColor = m_BackgroundImage.color;
        Hide();
    }

    public void RegisterAction()
    {
        var actionButton = Instantiate(m_ActionButtonPrefab, transform);
        m_ActionButtons.Add(actionButton);
    
    }

    public void ClearActions()
    {
        for (int i = m_ActionButtons.Count - 1; i >= 0; i--)
        {
            Destroy(m_ActionButtons[i].gameObject);
            m_ActionButtons.RemoveAt(i);

        }
    }

    public void Show()
    {
        m_BackgroundImage.color = m_OriginalBackgroundColor;
    }

    public void Hide()
    {
        m_BackgroundImage.color = new Color(0, 0, 0, 0); 
    }

}