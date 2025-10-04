using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;

    private GameRule m_gameRule;

    public void Init(GameRule gameRule)
    {
        m_gameRule = gameRule;
        Destroy(gameObject, lifeTime);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // check if colliding with enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // check if it have EnemyBehaviour script using trygetcomponent

            if (collision.gameObject.TryGetComponent(out EnemyBehaviour enemyBehavior))
                m_gameRule.EnemyHit(collision.gameObject);

            Destroy(this.gameObject);
        }
    }
}
