using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarryBackgroundMaker : MonoBehaviour
{
    public float minStarSize = 2f;
    public float maxStarSize = 7f;
    public float minStarSpeed = 10f;
    public float maxStarSpeed = 30f;

    public float spawnInterval = 0.5f;

    private RectTransform rectTransform;

    private float minY;
    private float maxY;
    public List<GameObject> starPrefabs;
    private float m_timePassed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        minY = -rectTransform.rect.height / 2f;
        maxY = rectTransform.rect.height / 2f;

        m_timePassed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // places stars randomly in the background every 0.5 second
        if (m_timePassed >= spawnInterval)
        {
            float speed = Random.Range(minStarSpeed, maxStarSpeed);
            float size = Random.Range(minStarSize, maxStarSize);
            float rotation = Random.Range(0f, 360f);

            GameObject star = Instantiate(starPrefabs[Random.Range(0, starPrefabs.Count)], this.transform, false);
            RectTransform starRect = star.GetComponent<RectTransform>();
            starRect.anchoredPosition = new Vector2(10f, Random.Range(minY, maxY));

            StarUIScript starScript = star.GetComponent<StarUIScript>();
            starScript.Init(size, speed);
            starRect.localRotation = Quaternion.Euler(0f, 0f, rotation);

            Destroy(star, 20f);

            m_timePassed = 0f;
        }

        m_timePassed += Time.deltaTime;
    }
}
