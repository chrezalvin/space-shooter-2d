using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject playerShield;

    public GameRule gameRule;

    public Camera mainCamera;

    public float fireColdown = 0.5f;

    public AudioClip shootSfx;
    public AudioClip playerHitSfx;
    public AudioClip playerShieldUpSfx;
    public AudioClip playerShieldDownSfx;

    public AudioClip shootRelatedBuffSfx;

    public float knockbackForce = 10f;
    public float knockbackDuration = 0.1f;

    private SpriteRenderer m_spriteRenderer;
    private Color m_originalColor;

    private Rigidbody2D m_rigidbody2D;

    private float nextireTime = 0f;

    private float m_halfPlyerWidth;
    private float m_halfPlayerHeight;

    private bool m_tripleShot = false;

    void Start()
    {
        // calculate half width and height of player sprite
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        if (m_spriteRenderer != null)
        {
            m_halfPlyerWidth = m_spriteRenderer.bounds.size.x / 2f;
            m_halfPlayerHeight = m_spriteRenderer.bounds.size.y / 2f;
            m_originalColor = m_spriteRenderer.color;
        }

        // disable shield at start
        if (playerShield != null)
            playerShield.SetActive(false);
    }

    IEnumerator KnockbackCoroutine(Vector2 source, float duration)
    {
        Vector2 knockbackDir = ((Vector2)transform.position - source).normalized;
        float elapsed = 0f;

        m_spriteRenderer.color = Color.red;
        while (elapsed < duration)
        {
            transform.position += (Vector3)(knockbackDir * knockbackForce * Time.deltaTime);
            elapsed += Time.deltaTime;

            yield return null;
        }
        m_spriteRenderer.color = m_originalColor;
    }

    void ApplyKnockback(Vector2 source)
    {
        Vector2 knockbackDir = (this.transform.position - (Vector3)source).normalized;
        m_rigidbody2D.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);

        StartCoroutine(KnockbackCoroutine(source, knockbackDuration));
    }

    public void SetShield(bool active)
    {
        if (playerShield)
        {
            playerShield.SetActive(active);
            if (playerShieldUpSfx != null)
                AudioSource.PlayClipAtPoint(active ? playerShieldUpSfx : playerShieldDownSfx, Camera.main.transform.position);
        }
    }

    public void SetRapidFire(bool isRapid)
    {
        if (isRapid) {
            fireColdown = 0.2f;
            if (shootRelatedBuffSfx != null)
                AudioSource.PlayClipAtPoint(shootRelatedBuffSfx, Camera.main.transform.position);            
        }
        else
            fireColdown = 0.5f;
    }

    public void SetTripleShot(bool isTriple)
    {
        m_tripleShot = isTriple;
        if (isTriple && shootRelatedBuffSfx != null)
            AudioSource.PlayClipAtPoint(shootRelatedBuffSfx, Camera.main.transform.position);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player collided with " + collision.name);

        if (collision.CompareTag("Enemy"))
        {
            if (!gameRule.PlayerGetHit(collision.gameObject))
                return; // no knockback if shield is active
            
            if (playerHitSfx != null)
                AudioSource.PlayClipAtPoint(playerHitSfx, Camera.main.transform.position);
        
            ApplyKnockback(collision.transform.position);
        }

        if (collision.CompareTag("Buff"))
        {
            gameRule.ApplyBuff(collision.gameObject);
        }
    }

    void ClampToScreen()
    {
        Vector3 currentPos = this.transform.position;

        Vector3 min = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 max = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

        currentPos.x = Mathf.Clamp(currentPos.x, min.x + m_halfPlyerWidth, max.x - m_halfPlyerWidth);
        currentPos.y = Mathf.Clamp(currentPos.y, min.y + m_halfPlayerHeight, max.y - m_halfPlayerHeight);

        this.transform.position = currentPos;
    }

    void Shoot()
    {
        if (m_tripleShot)
        {
            // if triple shot, shoot 3 bullets in a spread pattern -15, 0, +15 degrees
            for (int i = -1; i <= 1; i++)
            {
                float angleOffset = i * 15f;
                Quaternion bulletRotation = firePoint.rotation * Quaternion.Euler(0, 0, angleOffset);
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
                BulletScript bulletScript = bullet.GetComponent<BulletScript>();
                bulletScript.Init(gameRule);
            }
        }
        else
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            BulletScript bulletScript = bullet.GetComponent<BulletScript>();
            bulletScript.Init(gameRule);
        }

        Debug.Log("Shoot");

        if (shootSfx != null)
            AudioSource.PlayClipAtPoint(shootSfx, Camera.main.transform.position);
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
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextireTime)
        {
            Shoot();
            nextireTime = Time.time + fireColdown;
        }

        this.transform.position += moveSpeed * Time.deltaTime * new Vector3(movement.x, movement.y, 0f);
        
        ClampToScreen();
    }
}
