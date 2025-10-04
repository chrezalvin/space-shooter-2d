public class PlayerStat
{
    public int maxHealth = 5;
    public int currentHealth;

    public float moveSpeed = 5f;
    public float fireRate = 0.5f; // shots per second

    public float rapidShotDuration = 0f; // in seconds
    public float multiShotDuration = 0f; // in seconds


    protected int m_currentHealth;
    protected float m_currentMoveSpeed;
    protected float m_currentFireRate;
    protected float m_rapidShotDuration;
    protected float m_multiShotDuration;

    public PlayerStat()
    {
        currentHealth = maxHealth;
    }
}