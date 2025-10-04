using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public GameObject healthBarPrefab;
    public GameObject healthBarContainer;

    public GameObject buffContainer;
    public GameObject buffIconPrefab;
    public Sprite tripleShotBuffIcon;
    public Sprite rapidShotBuffIcon;
    public Sprite shieldBuffIcon;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverScoreText;

    private GameObject m_tripleShotIcon;
    private GameObject m_rapidShotIcon;
    private GameObject m_shieldIcon;

    public void InitializeHPBar(int health)
    {
        // clear all existing health bars
        foreach (Transform child in healthBarContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // instantiate health bars according to health
        for (int i = 0; i < health; i++)
        {
            Instantiate(healthBarPrefab, healthBarContainer.transform);
        }
    }

    public void UpdateHPBar(int health)
    {
        int count = healthBarContainer.transform.childCount;

        for (int i = 0; i < count; i++)
        {
            Transform child = healthBarContainer.transform.GetChild(i);
            child.gameObject.SetActive(i < health);
        }
    }

    public void InitializeScore(int score = 0)
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateGameOverScore(int score)
    {
        gameOverScoreText.text = "Score: " + score.ToString();
    }

    public void InitializeBuffIcons()
    {
        // clear all existing buff icons
        foreach (Transform child in buffContainer.transform)
        {
            Destroy(child.gameObject);
        }

        m_tripleShotIcon = null;
        m_rapidShotIcon = null;
        m_shieldIcon = null;
    }

    public void AddTripleShotIcon()
    {
        if (m_tripleShotIcon == null)
        {
            m_tripleShotIcon = Instantiate(buffIconPrefab, buffContainer.transform);
            m_tripleShotIcon.GetComponent<UnityEngine.UI.Image>().sprite = tripleShotBuffIcon;
        }
    }

    public void RemoveTripleShotIcon()
    {
        if (m_tripleShotIcon != null)
        {
            Destroy(m_tripleShotIcon);
            m_tripleShotIcon = null;
        }
    }

    public void UpdateTripleShotIconDuration(float currentDuration, float maxDuration)
    {
        if (m_tripleShotIcon != null)
        {
            float fillAmount = Mathf.Clamp01(currentDuration / maxDuration);
            m_tripleShotIcon.GetComponent<UnityEngine.UI.Image>().fillAmount = fillAmount;
        }
    }

    public void AddRapidShotIcon()
    {
        if (m_rapidShotIcon == null)
        {
            m_rapidShotIcon = Instantiate(buffIconPrefab, buffContainer.transform);
            m_rapidShotIcon.GetComponent<UnityEngine.UI.Image>().sprite = rapidShotBuffIcon;
        }
    }

    public void RemoveRapidShotIcon()
    {
        if (m_rapidShotIcon != null)
        {
            Destroy(m_rapidShotIcon);
            m_rapidShotIcon = null;
        }
    }

    public void UpdateRapidShotIconDuration(float currentDuration, float maxDuration)
    {
        if (m_rapidShotIcon != null)
        {
            float fillAmount = Mathf.Clamp01(currentDuration / maxDuration);
            m_rapidShotIcon.GetComponent<UnityEngine.UI.Image>().fillAmount = fillAmount;
        }
    }

    public void AddShieldIcon()
    {
        if (m_shieldIcon == null)
        {
            m_shieldIcon = Instantiate(buffIconPrefab, buffContainer.transform);
            m_shieldIcon.GetComponent<UnityEngine.UI.Image>().sprite = shieldBuffIcon;
        }
    }

    public void RemoveShieldIcon()
    {
        if (m_shieldIcon != null)
        {
            Destroy(m_shieldIcon);
            m_shieldIcon = null;
        }
    }

    public void AddBuffIcon(string buffType)
    {
        Sprite iconSprite = null;
        switch (buffType)
        {
            case "TripleShot":
                iconSprite = tripleShotBuffIcon;
                break;
            case "RapidShot":
                iconSprite = rapidShotBuffIcon;
                break;
            case "Shield":
                iconSprite = shieldBuffIcon;
                break;
            default:
                Debug.LogWarning("Unknown buff type: " + buffType);
                return;
        }

        GameObject icon = Instantiate(buffIconPrefab, buffContainer.transform);
        icon.GetComponent<UnityEngine.UI.Image>().sprite = iconSprite;
    }

    public void Init(int health = 0, int score = 0)
    {
        InitializeHPBar(health);
        InitializeScore(score);
        InitializeBuffIcons();
    }
}
