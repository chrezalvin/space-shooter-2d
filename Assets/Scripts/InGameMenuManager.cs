using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameMenuManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject gameOverMenu;

    public AudioClip togglePauseMenuSfx;

    public void OpenPauseMenu()
    {
        // if game over menu is active, do nothing
        if (gameOverMenu.activeSelf) return;

        // if scene is in MainMenu, do nothing
        if (SceneManager.GetActiveScene().name == "MainMenu") return;

        pauseMenu.SetActive(true);
        if (togglePauseMenuSfx != null)
            AudioSource.PlayClipAtPoint(togglePauseMenuSfx, Camera.main.transform.position);
        Time.timeScale = 0f; // Pause the game
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }

    public void OpenGameOverMenu()
    {
        gameOverMenu.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void GoBackToMainMenu()
    {
        Time.timeScale = 1f; // Resume the game before going back to main menu
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void RestartGame()
    {
        // restart by reloading the battle scene
        Time.timeScale = 1f;
        SceneManager.LoadScene("BattleScene");
    }

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
    }

    void Update()
    {
        // listen for escape key to toggle pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
            if (pauseMenu.activeSelf)
                ClosePauseMenu();
            else
                OpenPauseMenu();
    } 
}
