using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStatUI : MonoBehaviour
{
    public GameObject healthBarContainer;
    public GameObject healthBarPrefab;
    public GameObject buffIconContainer;
    public GameObject buffIconPrefab;
    public TextMeshProUGUI textScore;

    private int m_maxHP;

    private List<GameObject> m_healthBars = new List<GameObject>();

    private Dictionary<BuffType, BuffIconBehaviour> m_buffIcons = new Dictionary<BuffType, BuffIconBehaviour>();

    private int m_currentHP;


    public void Init(int maxHP)
    {
        m_maxHP = maxHP;
        m_currentHP = maxHP;

        // remove all existing health bars
        foreach (Transform child in healthBarContainer.transform)
            Destroy(child.gameObject);

        for (int iii = 0; iii < maxHP; ++iii)
            m_healthBars.Add(Instantiate(healthBarPrefab, healthBarContainer.transform));

        LayoutRebuilder.ForceRebuildLayoutImmediate(healthBarContainer.GetComponent<RectTransform>());
    }

    void Update()
    {
        foreach (var buff in m_buffIcons)
            buff.Value.UpdateTimer();
    }

    public void UpdateHP(int newHP)
    {
        if (newHP < 0 || newHP > m_maxHP)
            return;

        m_currentHP = newHP;

        for (int iii = 0; iii < m_maxHP; ++iii)
            m_healthBars[iii].SetActive(iii < m_currentHP);
    }

    public void UpdateScore(int newScore)
    {
        textScore.text = "Score: " + newScore.ToString();
    }

    public void AddBuff(ActiveBuff activeBuff)
    {
        BuffType buffType = activeBuff.buff.GetBuffType();

        if (!m_buffIcons.ContainsKey(buffType))
        {
            GameObject buffIcon = Instantiate(buffIconPrefab, buffIconContainer.transform);
            BuffIconBehaviour buffIconBehaviour = buffIcon.GetComponent<BuffIconBehaviour>();
            if (buffIconBehaviour)
                m_buffIcons.Add(buffType, buffIconBehaviour);
            else
                Debug.LogError("Buff icon prefab does not have a BuffIconBehaviour component.");
        }

        m_buffIcons[buffType].Bind(activeBuff);

        LayoutRebuilder.ForceRebuildLayoutImmediate(buffIconContainer.GetComponent<RectTransform>());
    }
    
    public void RemoveBuff(ActiveBuff activeBuff)
    {
        BuffType buffType = activeBuff.buff.GetBuffType();

        if (m_buffIcons.ContainsKey(buffType))
        {
            Destroy(m_buffIcons[buffType].gameObject);
            m_buffIcons.Remove(buffType);
        }
        else
            Debug.LogWarning($"Trying to remove buff icon for buff type {buffType}, but it does not exist in the current icons.");
    }
}
