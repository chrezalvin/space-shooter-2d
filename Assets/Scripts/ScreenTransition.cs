using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenTransition : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    public void StartGame(string sceneName)
    {
        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        yield return StartCoroutine(Fade(0f, 1f));

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator Fade(float startAlpha, float endAlpha) {
        float time = 0f;
        Color color = fadeImage.color;

        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            fadeImage.color = color;

            time += Time.deltaTime;
            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
