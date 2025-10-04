using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarUIScript : MonoBehaviour
{
    private float m_speed = 20f;
    private float m_size = 5f;
    private RectTransform rectTransform;
    private float m_randomOffsetDeg;

    // Start is called before the first frame update
    public void Init(float size, float speed)
    {
        m_speed = speed;
        m_size = size;
        m_randomOffsetDeg = Random.Range(0f, 180f);

        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(size, size);
    }

    // Update is called once per frame
    void Update()
    {
        // move to the left
        rectTransform.anchoredPosition += Vector2.left * m_speed * Time.deltaTime;

        // pulsing effect with half the size
        float scale = m_size * Mathf.Sin(Time.time * 4f + m_randomOffsetDeg) / 6f + m_size;
        rectTransform.sizeDelta = new Vector3(scale, scale, 1f);
    }
}
