using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffIconBehaviour : MonoBehaviour
{
    [SerializeField] private Image iconImg;

    private ActiveBuff m_activeBuff;

    public void Bind(ActiveBuff activeBuff)
    {
        m_activeBuff = activeBuff;
        iconImg.sprite = activeBuff.buff.GetIcon();
    }

    // Update is called once per frame
    public void UpdateTimer()
    {
        if (m_activeBuff == null || m_activeBuff.ignoreTimer || m_activeBuff.totalDuration <= 0f) return;

        float ratio = Mathf.Clamp01(m_activeBuff.remainingTime / m_activeBuff.totalDuration);
        iconImg.fillAmount = ratio;  
    }
}
