using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BarSliderUI2 : MonoBehaviour
{
    public UnityEvent<int, int> test;

    public Image fillBar;
    public Image background;

    /// <summary>
    ///  Initializes the bar with default colors (green fill, red background).
    /// </summary>
    public void Init()
    {
        fillBar.color = Color.green;
        background.color = Color.red;

        test.Invoke(1, 2);
    }

    /// <summary>
    /// Initializes the bar with specified fill and background colors.
    /// </summary>
    public void Init(Color fillColor, Color backgroundColor)
    {
        fillBar.color = fillColor;
        background.color = backgroundColor;
    }

    /// <summary>
    /// Sets the amount of the bar based on total and current values.
    /// </summary>
    public void SetAmount(float total, float current)
    {
        if (total == 0f)
            return;

        float amount = current / total;
        amount = Mathf.Clamp01(amount);
        fillBar.fillAmount = amount;
    }

    /// <summary>
    /// Sets the amount of the bar directly using a normalized value (0 to 1).
    /// </summary>
    public void SetAmount(float amount)
    {
        amount = Mathf.Clamp01(amount);
        fillBar.fillAmount = amount;
    }
}
