using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarSliderUI : MonoBehaviour
{
    public Sprite fillBarIcon = null;
    [Range(0f, 1f)]
    public float percentage = 1f;
    public Image fillBar;
    public GameObject background;
    public GameObject iconObj;

    private Image m_iconImage;

    void Start()
    {
        m_iconImage = iconObj.GetComponent<Image>();
    }

    void OnValidate()
    {
        if (fillBarIcon)
        {
            Image iconImage = iconObj.GetComponent<Image>();
            if (iconImage)
                iconImage.sprite = fillBarIcon;
        }

        iconObj.SetActive(fillBarIcon != null);
    }

    public void SetPercentage(float percentage)
    {
        percentage = Mathf.Clamp01(percentage);
        fillBar.fillAmount = percentage;
    }

    public void SetIcon(Sprite icon)
    {
        if (icon)
            m_iconImage.sprite = icon;
    
        iconObj.SetActive(icon != null);
    }
}
