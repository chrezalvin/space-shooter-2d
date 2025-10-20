using UnityEngine;

public class EnemySpawnManager : SpawnManager
{
    public GameObject enemyPrefab;

    public float baseMinEnemySpeed = 1f;
    public float baseMaxEnemySpeed = 5f;

    public float baseMinEnemySize = 0.5f;
    public float baseMaxEnemySize = 2f;

    public int baseMinSpawnCount = 1;
    public int baseMaxSpawnCount = 3;

    public float baseSpawnInterval = 2f;

    private float m_timer = 0f;

    // Update is called once per frame
    protected override void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer >= baseSpawnInterval)
        {
            m_timer = 0f;
            int spawnCount = Random.Range(baseMinSpawnCount, baseMaxSpawnCount + 1);
            for (int iii = 0; iii < spawnCount; ++iii)
                SpawnEnemy();

            Debug.Log($"Spawned {spawnCount} enemies.");
        }

        base.Update();
    }

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
        {
            enemyBehaviour.Init(
                Random.Range(baseMinEnemySpeed, baseMaxEnemySpeed),
                Random.Range(baseMinEnemySize, baseMaxEnemySize)
            );
        }
        else
            Debug.LogError("Enemy prefab does not have an EnemyBehaviour component.");

        return enemy;
    }
}
