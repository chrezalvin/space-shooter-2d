using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreenUI : MonoBehaviour
{
    public BarSliderUI2 healthBar;

    public void Init()
    {
        healthBar.Init();
    }

    public void SetHealth(float total, float current)
    {
        healthBar.SetAmount(total, current);
    }
}
