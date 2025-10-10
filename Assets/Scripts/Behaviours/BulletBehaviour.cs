using System;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public static event Action<EnemyBehaviour> OnEnemyHit;

    public float speed = 10f;
    public float lifeTime = 2f;

    public int maxPierceCount = 0;

    private int m_currentPierceCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // check if colliding with enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyBehaviour enemyBehaviour = collision.gameObject.GetComponent<EnemyBehaviour>();
            OnEnemyHit?.Invoke(enemyBehaviour);
            m_currentPierceCount++;            

            if (m_currentPierceCount > maxPierceCount)
                Destroy(this.gameObject);
        }
    }
}