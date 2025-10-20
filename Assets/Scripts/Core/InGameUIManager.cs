using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    public GameStatUI gameStatUI;

    public GameObject gameOverPanel;
    public GameObject pauseMenuPanel;
    public TextMeshProUGUI gameOverScoreText;

    private void OnEnable()
    {
        GameRule.OnHPChanged += gameStatUI.UpdateHP;
        ScoreManager.OnScoreChanged += gameStatUI.UpdateScore;
        BuffManager.OnBuffAdded += gameStatUI.AddBuff;
        BuffManager.OnBuffRemoved += gameStatUI.RemoveBuff;
    }

    private void OnDisable()
    {
        GameRule.OnHPChanged -= gameStatUI.UpdateHP;
        ScoreManager.OnScoreChanged -= gameStatUI.UpdateScore;
        BuffManager.OnBuffAdded -= gameStatUI.AddBuff;
        BuffManager.OnBuffRemoved -= gameStatUI.RemoveBuff;
    }

    // Start is called before the first frame update
    public void Init(int maxHP, int initialScore = 0)
    {
        gameStatUI.Init(maxHP);

        // deactivate game over and pause menu panels just in case
        gameOverPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // toggle pause menu with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
            SetPauseMenu(!pauseMenuPanel.activeSelf);
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
