using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public static Action<EnemyBehaviour> OnPlayerHit;
    public static Action<BuffBehaviour> OnBuffCollected;

    private MovementBehaviour m_movementBehaviour;

    protected Camera m_mainCamera;

    protected ShipBehaviour m_shipBehaviour;

    // sound effects
    protected SpriteRenderer m_spriteRenderer;

    protected float m_halfPlayerWidth = 0f;
    protected float m_halfPlayerHeight = 0f;

    public virtual void Init(Camera camera, Ship ship, MovementBehaviour movementBehaviour)
    {
        m_mainCamera = camera;

        m_shipBehaviour = GetComponent<ShipBehaviour>();
        m_movementBehaviour = movementBehaviour;

        if (m_shipBehaviour)
            m_shipBehaviour.Init(ship);
        else
            Debug.LogError("Player object does not have a ShipBehaviour component.");

        m_spriteRenderer = GetComponent<SpriteRenderer>();
        if (m_spriteRenderer)
        {
            m_halfPlayerWidth = m_spriteRenderer.bounds.size.x / 2f;
            m_halfPlayerHeight = m_spriteRenderer.bounds.size.y / 2f;
        }
        else
            Debug.LogError("Player object does not have a SpriteRenderer component.");
    }

    protected void Update()
    {
        Vector2 movement = m_movementBehaviour.GetMovement();

        // updates rotation
        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;

            angle -= 90f;

            float snappedAngle = Mathf.Round(angle / 45f) * 45f;

            this.transform.rotation = Quaternion.Euler(0f, 0f, snappedAngle);
        }

        // checks for shooting
        if (m_movementBehaviour.IsShooting())
            m_shipBehaviour.TryShoot();

        this.transform.position += m_shipBehaviour.GetShip().GetMovementSpeed() * Time.deltaTime * new Vector3(movement.x, movement.y, 0f);


        ClampToScreen();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player collided with " + collision.name);

        if (collision.CompareTag("Enemy"))
        {
            EnemyBehaviour enemyBehavior = collision.GetComponent<EnemyBehaviour>();
            OnPlayerHit?.Invoke(enemyBehavior);
        }

        if (collision.CompareTag("Buff"))
        {
            BuffBehaviour buffBehavior = collision.GetComponent<BuffBehaviour>();
            OnBuffCollected?.Invoke(buffBehavior);
        }
    }

    public IEnumerator KnockbackCoroutine(Vector2 source, float duration)
    {
        Vector2 knockbackDir = ((Vector2)transform.position - source).normalized;
        float elapsed = 0f;

        m_spriteRenderer.color = Color.red;
        while (elapsed < duration)
        {
            transform.position += (Vector3)(knockbackDir * 10f * Time.deltaTime);
            elapsed += Time.deltaTime;

            yield return null;
        }

        // set to white for now
        m_spriteRenderer.color = Color.white;
    }

    public void ApplyKnockback(Vector2 source)
    {
        StartCoroutine(KnockbackCoroutine(source, 0.1f));
    }

    protected void ClampToScreen()
    {
        Vector3 currentPos = this.transform.position;

        Vector3 min = m_mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 max = m_mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

        currentPos.x = Mathf.Clamp(currentPos.x, min.x + m_halfPlayerWidth, max.x - m_halfPlayerWidth);
        currentPos.y = Mathf.Clamp(currentPos.y, min.y + m_halfPlayerHeight, max.y - m_halfPlayerHeight);

        this.transform.position = currentPos;
    }

    public ShipBehaviour GetShipBehaviour() { return m_shipBehaviour; }
}