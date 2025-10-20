using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffInfoManager : MonoBehaviour
{
    public TextMeshProUGUI buffName;
    public TextMeshProUGUI buffDesc;

    public Button closeButton;

    public Image buffIcon;

    private Action m_onCloseCallback;

    public void Init(Action OnClose = null)
    {
        m_onCloseCallback = OnClose;
        closeButton.onClick.RemoveAllListeners();

        closeButton.onClick.AddListener(Close);
    }

    public void Set(Buff buff)
    {
        buffName.text = buff.GetBuffName();
        buffDesc.text = buff.GetDescription();
        buffIcon.sprite = buff.GetIcon();
    }

    public void Close()
    {
        m_onCloseCallback?.Invoke();
    }
}
