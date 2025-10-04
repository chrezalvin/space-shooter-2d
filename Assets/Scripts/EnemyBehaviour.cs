using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float enemyScale = 1f;

    public GameObject explosionPrefab; 
    public AudioClip explosionSfx;

    public void Init(float speed, float scale)
    {
        moveSpeed = speed;
        enemyScale = scale;

        this.transform.localScale = new Vector3(enemyScale, enemyScale, 1f);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // speed already set in Init or default value
    }

    // Update is called once per frame
    void Update()
    {
        // move forward
        this.transform.position += moveSpeed * Time.deltaTime * this.transform.up;
    }

    public void Die()
    {
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
            ExplosionScript explosionScript = explosion.GetComponent<ExplosionScript>();
            if (explosionScript)
                explosionScript.Init(enemyScale);
        }

        if (explosionSfx != null)
            AudioSource.PlayClipAtPoint(explosionSfx, Camera.main.transform.position); 

        Destroy(this.gameObject);
    }
}
