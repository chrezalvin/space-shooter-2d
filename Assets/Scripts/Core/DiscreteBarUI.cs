using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscreteBarUI : MonoBehaviour
{
    public Sprite icon = null;
    public GameObject iconImageObj;
    public GameObject fillUnitPrefab;
    public GameObject fillUnitContainer;

    private int m_currentValue = 0;

    public void SetValue(int value)
    {
        // remove existing units
        foreach (Transform child in fillUnitContainer.transform)
            Destroy(child.gameObject);

        m_currentValue = value;
        for (int iii = 0; iii < m_currentValue; ++iii)
            Instantiate(fillUnitPrefab, fillUnitContainer.transform);

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fillUnitContainer.transform);
    }
    
    void OnValidate()
    {
        if (icon)
        {
            Image iconImage = this.iconImageObj.GetComponent<Image>();
            if (iconImage)
                iconImage.sprite = icon;
        }

        iconImageObj.SetActive(icon != null);
    }
}
