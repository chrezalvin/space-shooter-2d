using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : SpawnManager
{
    public GameObject enemyPrefab;

    public float minEnemySpeed = 1f;
    public float maxEnemySpeed = 5f;

    public float minEnemySize = 0.5f;
    public float maxEnemySize = 2f;

    public int minSpawnCount = 1;
    public int maxSpawnCount = 3;

    public float spawnInterval = 2f;

    private float m_timer = 0f;

    private GameObject SpawnEnemy()
    {
        Vector2 from = GetRandomPointOnEdge(m_boundary);
        Vector2 to;

        do
        {
            to = GetRandomPointOnEdge(m_boundary);
        } while (Mathf.Approximately(from.x, to.x) || Mathf.Approximately(from.y, to.y)); // avoid vertical line

        GameObject enemy = this.SpawnObject(enemyPrefab, from, to);
        EnemyBehaviour enemyBehaviour = enemy.GetComponent<EnemyBehaviour>();
        if (enemyBehaviour)
            enemyBehaviour.Init(
                Random.Range(minEnemySpeed, maxEnemySpeed),
                Random.Range(minEnemySize, maxEnemySize)
            );
        else
            Debug.LogError("Enemy prefab does not have an EnemyBehaviour component.");

        return enemy;
    }

    // Update is called once per frame
    public override void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer >= spawnInterval)
        {
            m_timer = 0f;
            int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);
            for (int iii = 0; iii < spawnCount; ++iii)
                SpawnEnemy();

            Debug.Log($"Spawned {spawnCount} enemies.");
        }

        base.Update();
    }
}
