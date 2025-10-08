using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffInfoManager : MonoBehaviour
{
    public GameObject buffInfoPanel;
    public TextMeshProUGUI buffName;
    public TextMeshProUGUI buffDesc;

    public Image buffIcon;

    void OnEnable()
    {
        ShipSelectionManager.OnBuffSelected += Set;        
    }

    void OnDisable()
    {
        ShipSelectionManager.OnBuffSelected -= Set;        
    }

    public void Set(Buff buff)
    {
        buffName.text = buff.GetBuffName();
        buffDesc.text = buff.GetDescription();
        buffIcon.sprite = buff.GetIcon();

        buffInfoPanel.SetActive(true);
    }

    public void CloseUI()
    {
        Debug.Log("Close buff info panel");
        buffInfoPanel.SetActive(false);
    }
}
