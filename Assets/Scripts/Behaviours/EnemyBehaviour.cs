using System;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public static Action OnEnemyDeath;

    public GameObject explosionPrefab;

    private Enemy m_enemy;

    public void Init(float speed = 3f, float scale = 1f)
    {
        m_enemy = new Enemy(scale, speed);
        this.transform.localScale = new Vector3(m_enemy.size, m_enemy.size, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        // move forward
        this.transform.position += m_enemy.speed * Time.deltaTime * this.transform.up;
    }

    public void Die()
    {
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
            explosion.transform.localScale = new Vector3(m_enemy.size, m_enemy.size, 1f);
        }

        OnEnemyDeath?.Invoke();

        Destroy(this.gameObject);
    }
    
    public Enemy GetEnemy()
    {
        return m_enemy;
    }
}
