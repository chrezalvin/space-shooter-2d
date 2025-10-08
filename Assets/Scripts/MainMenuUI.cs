using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public float fadeDuration = .5f;

    // uses mask image to fade in and out effect
    public RectMask2D maskImg;

    public TextMeshProUGUI versionText;

    public GameObject chooseShipMenu;

    private float m_currentMaskFadeLeft;

    public void LoadBattleScene()
    {
        StartCoroutine(FadeAndLoadScene("BattleScene"));
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void OpenChooseShipMenu()
    {
        // open choose ship menu
        if (chooseShipMenu)
            chooseShipMenu.gameObject.SetActive(true);
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        yield return StartCoroutine(Fade(m_currentMaskFadeLeft, 0f));

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator Fade(float startPadLeft, float endPadLeft)
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            float padLeft = Mathf.Lerp(startPadLeft, endPadLeft, t);
            maskImg.padding = new Vector4(padLeft, 0, 0, 0);

            time += Time.deltaTime;
            yield return null;
        }
    }

    void Start()
    {
        m_currentMaskFadeLeft = maskImg.padding.x;
        versionText.text = "version: " + Application.version;
    }
}
