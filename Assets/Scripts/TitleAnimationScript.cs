using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimationScript : MonoBehaviour
{
    public float floatRangePx = 2f;
    public float rotateRangeDeg = 5f;

    public float scaleRange = 0.1f;
    
    private Vector3 m_initialPosition;

    private RectTransform m_rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();

        m_initialPosition = m_rectTransform.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // make the title UI float up and down, resize a bit bigger and smaller, and rotate a bit
        float newY = floatRangePx * Mathf.Sin(Time.time);
        float newRotZ = rotateRangeDeg * Mathf.Sin(Time.time);
        float newScale = 1f + scaleRange * Mathf.Sin(2 * Time.time);

        m_rectTransform.anchoredPosition = new Vector2(m_initialPosition.x, m_initialPosition.y + newY);
        m_rectTransform.localRotation = Quaternion.Euler(0f, 0f, newRotZ);
        transform.localScale = new Vector3(newScale, newScale, 1f);
    }
}
