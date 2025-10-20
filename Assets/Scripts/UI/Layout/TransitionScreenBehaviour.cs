using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionScreenBehaviour : MonoBehaviour
{
    public Image fillImage;

    public void TransitionScreen(float duration, Action onComplete = null, bool reverse = false)
    {
        if(reverse)
            StartCoroutine(Fill(duration, 1f, 0f, onComplete));
        else
            StartCoroutine(Fill(duration, 0f, 1f, onComplete));
    }

    public IEnumerator Fill(float duration, float start = 0f, float end = 1f, Action onComplete = null)
    {
        transform.SetAsLastSibling();
        float time = 0f;
        while (time < duration)
        {
            float t = time / duration;
            fillImage.fillAmount = Mathf.Lerp(start, end, t);

            time += Time.deltaTime;
            yield return null;
        }
        fillImage.fillAmount = 1f;

        onComplete?.Invoke();
    }

    void Start()
    {
        fillImage.fillAmount = 0f;
    }
}
