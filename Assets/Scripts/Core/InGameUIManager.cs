using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    public GameRule gameRule;

    public GameObject healthBarPrefab;
    public GameObject healthBarContainer;
    public GameObject buffContainer;
    public GameObject buffIconPrefab;
    public GameObject gameOverPanel;
    public GameObject pauseMenuPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverScoreText;

    private Dictionary<BuffType, BuffIconBehaviour> m_buffIcons = new Dictionary<BuffType, BuffIconBehaviour>();

    private void OnEnable()
    {
        GameRule.OnHPChanged += UpdateHPBar;
        ScoreManager.OnScoreChanged += UpdateScore;
        BuffManager.OnBuffAdded += OnBuffAdded;
        BuffManager.OnBuffRemoved += OnBuffRemoved;
    }

    private void OnDisable()
    {
        GameRule.OnHPChanged -= UpdateHPBar;
        ScoreManager.OnScoreChanged -= UpdateScore;
        BuffManager.OnBuffAdded -= OnBuffAdded;
        BuffManager.OnBuffRemoved -= OnBuffRemoved;
    }

    // Start is called before the first frame update
    public void Init(int maxHP, int initialScore = 0)
    {
        InitializeHPBar(maxHP);
        InitializeScore(initialScore);
        InitializeBuffIcons();

        // deactivate game over and pause menu panels just in case
        gameOverPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var buff in m_buffIcons)
            buff.Value.UpdateTimer();

        // toggle pause menu with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
            SetPauseMenu(!pauseMenuPanel.activeSelf);
    }

    public void OnBuffAdded(ActiveBuff ab)
    {
        BuffType buffType = ab.buff.GetBuffType();

        if (!m_buffIcons.ContainsKey(buffType))
        {
            GameObject buffIcon = Instantiate(buffIconPrefab, buffContainer.transform);
            BuffIconBehaviour buffIconBehaviour = buffIcon.GetComponent<BuffIconBehaviour>();
            if (buffIconBehaviour)
                m_buffIcons.Add(buffType, buffIconBehaviour);
            else
                Debug.LogError("Buff icon prefab does not have a BuffIconBehaviour component.");
        }

        m_buffIcons[buffType].Bind(ab);
    }

    public void OnBuffRemoved(ActiveBuff ab)
    {
        BuffType buffType = ab.buff.GetBuffType();

        if (m_buffIcons.ContainsKey(buffType))
        {
            Destroy(m_buffIcons[buffType].gameObject);
            m_buffIcons.Remove(buffType);
        }
        else
            Debug.LogWarning($"Trying to remove buff icon for buff type {buffType}, but it does not exist in the current icons.");
    }

    public void UpdateHPBar(int HP)
    {
        int count = healthBarContainer.transform.childCount;

        for (int iii = 0; iii < count; ++iii)
        {
            Transform child = healthBarContainer.transform.GetChild(iii);
            child.gameObject.SetActive(iii < HP);
        }
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void GameOver(int finalScore)
    {
        gameOverPanel.SetActive(true);
        gameOverScoreText.text = "Final Score: " + finalScore.ToString();

        // freeze the game
        Time.timeScale = 0f;
    }

    public void SetPauseMenu(bool pause)
    {
        pauseMenuPanel.SetActive(pause);

        // freeze/unfreeze the game
        Time.timeScale = pause ? 0f : 1f;
    }

    private void InitializeHPBar(int health)
    {
        // clear all existing health bars
        foreach (Transform child in healthBarContainer.transform)
            Destroy(child.gameObject);

        // instantiate health bars according to health
        for (int iii = 0; iii < health; ++iii)
            Instantiate(healthBarPrefab, healthBarContainer.transform);
    }

    private void InitializeScore(int score = 0)
    {
        scoreText.text = "Score: " + score.ToString();
    }

    private void InitializeBuffIcons()
    {
        // clear all existing buff icons
        foreach (Transform child in buffContainer.transform)
            Destroy(child.gameObject);
    }

    public void RestartGame()
    {
        // unfreeze the game
        Time.timeScale = 1f;

        // reload the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("BattleScene");
    }

    public void QuitToMainMenu()
    {
        // unfreeze the game
        Time.timeScale = 1f;

        // load the main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
