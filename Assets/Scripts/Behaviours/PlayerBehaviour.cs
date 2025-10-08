using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public static Action<EnemyBehaviour> OnPlayerHit;
    public static Action<BuffBehaviour> OnBuffCollected;

    private Camera m_mainCamera;

    private ShipBehaviour m_shipBehaviour;

    // sound effects
    private SpriteRenderer m_spriteRenderer;

    private float m_halfPlayerWidth = 0f;
    private float m_halfPlayerHeight = 0f;

    public void Init(Camera camera, Ship ship)
    {
        m_mainCamera = camera;

        m_shipBehaviour = GetComponent<ShipBehaviour>();
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

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(moveX, moveY).normalized;

        // updates rotation
        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;

            angle -= 90f;

            float snappedAngle = Mathf.Round(angle / 45f) * 45f;

            this.transform.rotation = Quaternion.Euler(0f, 0f, snappedAngle);
        }

        // checks for shooting
        if (Input.GetKeyDown(KeyCode.Space))
            m_shipBehaviour.TryShoot();

        this.transform.position += m_shipBehaviour.GetShip().GetMovementSpeed() * Time.deltaTime * new Vector3(movement.x, movement.y, 0f);

        ClampToScreen();
    }

    void OnTriggerEnter2D(Collider2D collision)
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

    IEnumerator KnockbackCoroutine(Vector2 source, float duration)
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

    void ClampToScreen()
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

// public class PlayerBehaviour : MonoBehaviour
// {
//     public static Action<EnemyBehaviour> OnPlayerHit;
//     public static Action<BuffBehaviour> OnBuffCollected;

//     public Camera mainCamera;

//     public GameObject bulletPrefab;
//     public GameObject playerShield;

//     public AudioClip playerHitSfx;

//     public List<Transform> mainFirePoint;
//     public List<Transform> secondaryFirePoint;

//     public AudioClip shieldUpSfx;
//     public AudioClip shieldDownSfx;

//     public AudioClip shootRelatedSfx;

//     // sound effects
//     public AudioClip shootSfx;

//     private Ship m_ship;
//     private SpriteRenderer m_spriteRenderer;
//     private float m_nextFireTime = 0f;

//     private float m_halfPlayerWidth = 0f;
//     private float m_halfPlayerHeight = 0f;

//     public void Init(Camera camera, Ship ship)
//     {
//         mainCamera = camera;
//         m_ship = ship;

//         m_spriteRenderer = GetComponent<SpriteRenderer>();
//         if (m_spriteRenderer)
//         {
//             m_halfPlayerWidth = m_spriteRenderer.bounds.size.x / 2f;
//             m_halfPlayerHeight = m_spriteRenderer.bounds.size.y / 2f;
//         }
//         else
//             Debug.LogError("Player object does not have a SpriteRenderer component.");

//         playerShield.SetActive(m_ship.shielded);
//     }

//     void Update()
//     {
//         float moveX = Input.GetAxisRaw("Horizontal");
//         float moveY = Input.GetAxisRaw("Vertical");

//         Vector2 movement = new Vector2(moveX, moveY).normalized;

//         // updates rotation
//         if (movement != Vector2.zero)
//         {
//             float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;

//             angle -= 90f;

//             float snappedAngle = Mathf.Round(angle / 45f) * 45f;

//             this.transform.rotation = Quaternion.Euler(0f, 0f, snappedAngle);
//         }

//         // checks for shooting
//         if (Input.GetKeyDown(KeyCode.Space) && Time.time >= m_nextFireTime)
//         {
//             Shoot();
//             m_nextFireTime = Time.time + m_ship.GetAttackSpeed();
//         }

//         this.transform.position += m_ship.GetMovementSpeed() * Time.deltaTime * new Vector3(movement.x, movement.y, 0f);

//         ClampToScreen();
//     }

//     void OnTriggerEnter2D(Collider2D collision)
//     {
//         Debug.Log("Player collided with " + collision.name);

//         if (collision.CompareTag("Enemy"))
//         {
//             EnemyBehaviour enemyBehavior = collision.GetComponent<EnemyBehaviour>();
//             OnPlayerHit?.Invoke(enemyBehavior);
//         }

//         if (collision.CompareTag("Buff"))
//         {
//             BuffBehaviour buffBehavior = collision.GetComponent<BuffBehaviour>();
//             OnBuffCollected?.Invoke(buffBehavior);
//         }
//     }

//     IEnumerator KnockbackCoroutine(Vector2 source, float duration)
//     {
//         Vector2 knockbackDir = ((Vector2)transform.position - source).normalized;
//         float elapsed = 0f;

//         m_spriteRenderer.color = Color.red;
//         while (elapsed < duration)
//         {
//             transform.position += (Vector3)(knockbackDir * 10f * Time.deltaTime);
//             elapsed += Time.deltaTime;

//             yield return null;
//         }

//         // set to white for now
//         m_spriteRenderer.color = Color.white;
//     }

//     public void ApplyKnockback(Vector2 source)
//     {
//         StartCoroutine(KnockbackCoroutine(source, 0.1f));
//     }

//     void ClampToScreen()
//     {
//         Vector3 currentPos = this.transform.position;

//         Vector3 min = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
//         Vector3 max = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

//         currentPos.x = Mathf.Clamp(currentPos.x, min.x + m_halfPlayerWidth, max.x - m_halfPlayerWidth);
//         currentPos.y = Mathf.Clamp(currentPos.y, min.y + m_halfPlayerHeight, max.y - m_halfPlayerHeight);

//         this.transform.position = currentPos;
//     }


//     void Shoot()
//     {
//         foreach (Transform firePoint in mainFirePoint)
//             Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

//         if (m_ship.isMultiShotActive)
//         {
//             // if triple shot, shoot from secondary fire points as well
//             foreach (Transform firePoint in secondaryFirePoint)
//                 Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
//         }

//         Debug.Log("Shoot");

//         if (shootSfx != null)
//             AudioSource.PlayClipAtPoint(shootSfx, Camera.main.transform.position);
//     }

//     public void ApplyShield(bool active)
//     {
//         AudioSource.PlayClipAtPoint(active ? shieldUpSfx : shieldDownSfx, mainCamera.transform.position);
//         m_ship.shielded = active;
//         playerShield.SetActive(active);
//     }

//     public void ApplyMultiShot(bool active)
//     {
//         if (active)
//             AudioSource.PlayClipAtPoint(shootRelatedSfx, mainCamera.transform.position);
//         m_ship.isMultiShotActive = active;
//     }

//     public void ApplyRapidShot(bool active)
//     {
//         if (active)
//             AudioSource.PlayClipAtPoint(shootRelatedSfx, mainCamera.transform.position);
//         m_ship.isRapidShotActive = active;
//     }
// }
