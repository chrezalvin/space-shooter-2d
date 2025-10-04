using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameRule : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject buffPrefab;
    public GameObject boundary;
    public GameObject playerPrefab;

    public InGameMenuManager inGameMenuManager;

    public GameUIManager gameUIManager;

    // range for enemy speed modifier
    public float minEnemySpeed = 1f;
    public float maxEnemySpeed = 5f;

    public float minEnemyScale = 0.5f;
    public float maxEnemyScale = 2f;

    public float spawnInterval = 2f;
    public int minSpawnCount = 1;
    public int maxSpawnCount = 3;

    public int playerHealth = 5;

    private int m_score;

    private List<GameObject> m_enemyList;
    private GameObject m_buffObject;
    private BoxCollider2D m_boundaryCollider;

    public float powerUpDuration = 5f;

    private float m_rapidShotDuration = 0f;
    private float m_multiShotDuration = 0f;

    private bool m_shieldActive = false;

    private PlayerScript m_playerScript;

    // Returns a random point on the edge of the rectangle defined by the given boundaries
    Vector2 GetRandomPointOnEdge(Bounds bounds)
    {
        int side = Random.Range(0, 4); // 0: left, 1: right, 2: top, 3: bottom
        switch (side)
        {
            case 0: // left
                return new Vector2(bounds.min.x, Random.Range(bounds.min.y, bounds.max.y));
            case 1: // right
                return new Vector2(bounds.max.x, Random.Range(bounds.min.y, bounds.max.y));
            case 2: // top
                return new Vector2(Random.Range(bounds.min.x, bounds.max.x), bounds.max.y);
            case 3: // bottom
                return new Vector2(Random.Range(bounds.min.x, bounds.max.x), bounds.min.y);
            default:
                return Vector2.zero; // should never reach here
        }
    }

    GameObject SpawnEnemy()
    {
        Bounds bounds = m_boundaryCollider.bounds;
        Vector2 p_a = GetRandomPointOnEdge(bounds);
        Vector2 p_b;

        do
        {
            p_b = GetRandomPointOnEdge(bounds);
        } while (Mathf.Approximately(p_a.x, p_b.x) || Mathf.Approximately(p_a.y, p_b.y)); // avoid vertical line

        Vector2 direction = (p_b - p_a).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

        GameObject enemy = Instantiate(enemyPrefab, p_a, rotation);
        EnemyBehaviour enemyBehavior = enemyPrefab.GetComponent<EnemyBehaviour>();

        float moveSpeed = Random.Range(minEnemySpeed, maxEnemySpeed);
        float scale = Random.Range(minEnemyScale, maxEnemyScale);

        enemyBehavior.Init(moveSpeed, scale);

        return enemy;
    }

    GameObject SpawnBuff()
    {
        Bounds bounds = m_boundaryCollider.bounds;
        Vector2 p_a = GetRandomPointOnEdge(bounds);
        Vector2 p_b;

        do
        {
            p_b = GetRandomPointOnEdge(bounds);
        } while (Mathf.Approximately(p_a.x, p_b.x) || Mathf.Approximately(p_a.y, p_b.y)); // avoid vertical line

        Vector2 direction = (p_b - p_a).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

        GameObject buff = Instantiate(buffPrefab, p_a, rotation);
        BuffBehaviour buffBehaviour = buff.GetComponent<BuffBehaviour>();

        float moveSpeed = Random.Range(minEnemySpeed, maxEnemySpeed);
        float scale = Random.Range(minEnemyScale, maxEnemyScale);

        buffBehaviour.Init(moveSpeed);

        return buff;
    }

    public void ApplyBuff(GameObject buff)
    {
        BuffBehaviour buffBehavior = buff.GetComponent<BuffBehaviour>();

        // Example: Apply a random buff
        int buffType = Random.Range(0, 3); // 0: RapidShot, 1: MultiShot, 2: Shield
        switch (buffType)
        {
            case 0:
                m_rapidShotDuration = powerUpDuration;
                gameUIManager.AddRapidShotIcon();
                m_playerScript.SetRapidFire(true);
                break;
            case 1:
                m_multiShotDuration = powerUpDuration;
                gameUIManager.AddTripleShotIcon();
                m_playerScript.SetTripleShot(true);
                break;
            case 2:
                m_shieldActive = true;
                gameUIManager.AddShieldIcon();
                m_playerScript.SetShield(true);
                break;
        }

        buffBehavior.Collected();
    }

    public void EnemyHit(GameObject enemy)
    {
        EnemyBehaviour enemyBehavior = enemy.GetComponent<EnemyBehaviour>();

        int score = Mathf.FloorToInt(enemyBehavior.enemyScale * enemyBehavior.moveSpeed);
        m_score += score == 0 ? 1 : score; // at least 1 point

        gameUIManager.UpdateScore(m_score);

        enemyBehavior.Die();

        Debug.Log("Enemy hit!");
    }

    public void GameOver()
    {
        gameUIManager.UpdateGameOverScore(m_score);
        inGameMenuManager.OpenGameOverMenu();
    }

    public bool PlayerGetHit(GameObject enemy)
    {
        // check if enemy is valid
        if (!enemy.TryGetComponent(out EnemyBehaviour enemyBehavior)) return false;

        EnemyHit(enemy);

        if (m_shieldActive)
        {
            m_shieldActive = false;
            m_playerScript.SetShield(false);
            gameUIManager.RemoveShieldIcon();

            return false; // no health lost
        }
        else
        {
            --playerHealth;
            gameUIManager.UpdateHPBar(playerHealth);

            if (playerHealth <= 0)
                GameOver();

            return true;
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_enemyList = new List<GameObject>();
        m_boundaryCollider = boundary.GetComponent<BoxCollider2D>();
        m_playerScript = playerPrefab.GetComponent<PlayerScript>();

        gameUIManager.Init(playerHealth, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Bounds bounds = m_boundaryCollider.bounds;

        // check if any enemy is out of boundary, use --iii to avoid shifting problem
        for (int iii = m_enemyList.Count - 1; iii >= 0; --iii)
        {
            GameObject enemy = m_enemyList[iii];
            if (enemy == null || !bounds.Contains(enemy.transform.position))
            {
                // destroy the enemy
                Destroy(enemy);
                m_enemyList.RemoveAt(iii);
            }
        }

        // check if buff object is out of boundary if it exists
        if (m_buffObject != null && !bounds.Contains(m_buffObject.transform.position))
        {
            Destroy(m_buffObject);
            m_buffObject = null;
        }

        // spawn n number of enemy per interval
        if (Time.frameCount % (int)(spawnInterval / Time.deltaTime) == 0)
        {
            for (int i = 0; i < Random.Range(minSpawnCount, maxSpawnCount + 1); i++)
                // add to enemy list
                m_enemyList.Add(SpawnEnemy());
        }

        // spawn buff object per interval with 10% chance
        if (m_buffObject == null && Time.frameCount % (int)(spawnInterval * 5f / Time.deltaTime) == 0)
        {
            // if (Random.Range(0f, 1f) <= 0.1f)
            m_buffObject = SpawnBuff();
        }


        if (m_rapidShotDuration > 0f)
        {
            m_rapidShotDuration = Mathf.Max(m_rapidShotDuration - Time.deltaTime, 0f);
            gameUIManager.UpdateRapidShotIconDuration(m_rapidShotDuration, powerUpDuration);
        }
        else
        {
            gameUIManager.RemoveRapidShotIcon();
            m_playerScript.SetRapidFire(false);
        }


        if (m_multiShotDuration > 0f)
        {
            m_multiShotDuration = Mathf.Max(m_multiShotDuration - Time.deltaTime, 0f);
            gameUIManager.UpdateTripleShotIconDuration(m_multiShotDuration, powerUpDuration);
        }
        else
        {
            gameUIManager.RemoveTripleShotIcon();
            m_playerScript.SetTripleShot(false);
        }
    }
}
