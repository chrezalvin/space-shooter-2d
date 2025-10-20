using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameRule : MonoBehaviour
{
    public static Action<int> OnHPChanged;

    public int difficultyIncreaseScoreTreshold = 200;

    [Range(1.0f, 5.0f), Header("Difficulty Growth Rate"), Tooltip("Higher value means faster difficulty growth")]
    public float difficultyGrowthRate = 1.2f;

    [Range(1, 3), Tooltip("Higher value means more difficult game")]
    public float difficultyIncreaseFactor = 1.5f;

    public int minScoreToEvolve = 300;

    public MovementBehaviour movementBehaviour;

    public BuffDatabase buffDatabase;
    public ShipDatabase shipDatabase;

    public ScoreManager scoreManager;
    public EnemySpawnManager enemySpawnManager;
    public BuffSpawnManager buffSpawnManager;
    public InGameUIManager inGameUIManager;
    public BuffManager buffManager;

    public Camera mainCamera;

    private Ship m_ship;
    private GameObject m_shipPrefab;

    private PlayerBehaviour m_playerBehaviour;

    private int m_currentHP;

    private int m_difficultyMultiplier = 1;

    private float m_enemyMinSpeedConstraint;
    private float m_enemyMaxSpeedConstraint;
    private float m_enemyMinSizeConstraint;
    private float m_enemyMaxSizeConstraint;
    private int m_enemyMinSpawnCountConstraint;
    private int m_enemyMaxSpawnCountConstraint;
    private float m_enemySpawnIntervalConstraint;

    private void OnEnable()
    {
        BulletBehaviour.OnEnemyHit += HandleEnemyHit;
        PlayerBehaviour.OnPlayerHit += HandlePlayerGetHit;
        PlayerBehaviour.OnBuffCollected += HandleBuffCollected;
        ScoreManager.OnScoreChanged += HandleScoreChanged;
    }

    private void OnDisable()
    {
        BulletBehaviour.OnEnemyHit -= HandleEnemyHit;
        PlayerBehaviour.OnPlayerHit -= HandlePlayerGetHit;
        PlayerBehaviour.OnBuffCollected -= HandleBuffCollected;
        ScoreManager.OnScoreChanged -= HandleScoreChanged;
    }

    private void HandleScoreChanged(int newScore)
    {
        if (newScore > minScoreToEvolve && !m_ship.isEvolved)
            m_playerBehaviour.GetShipBehaviour().ApplyEvolve(buffManager, buffDatabase);

        // triggered every treshold
        if (Mathf.FloorToInt(newScore / difficultyIncreaseScoreTreshold) > m_difficultyMultiplier)
        {
            // Debug.Log("Increasing difficulty multiplier.");
            m_difficultyMultiplier += 1;

            // // use natural logarithmic scale to increase difficulty
            AdjustDifficulty(m_difficultyMultiplier);
        }

    }

    private void AdjustDifficulty(int difficultyMultiplier)
    {
        float increaseFactor = 1 + (difficultyIncreaseFactor - 1) * (Mathf.Log(difficultyMultiplier + 1, 2.7182818f) / ((5f / difficultyGrowthRate) + Mathf.Log(difficultyMultiplier + 1, 2.7182818f)));

        // max is double the constraint valuer

        enemySpawnManager.baseMaxEnemySize = m_enemyMaxSizeConstraint * increaseFactor;
        enemySpawnManager.baseMinEnemySize = m_enemyMinSizeConstraint * increaseFactor;

        enemySpawnManager.baseMaxEnemySpeed = m_enemyMaxSpeedConstraint * increaseFactor;
        enemySpawnManager.baseMinEnemySpeed = m_enemyMinSpeedConstraint * increaseFactor;

        enemySpawnManager.baseMaxSpawnCount = Mathf.CeilToInt(m_enemyMaxSpawnCountConstraint * increaseFactor);
        enemySpawnManager.baseMinSpawnCount = Mathf.CeilToInt(m_enemyMinSpawnCountConstraint * increaseFactor);

        enemySpawnManager.baseSpawnInterval = m_enemySpawnIntervalConstraint / increaseFactor;
    }

    private void HandleBuffCollected(BuffBehaviour buffBehaviour)
    {
        if (buffBehaviour == null) return;

        // get buffs randomly
        // check if player have shield buff
        Buff buff = buffDatabase.GetRandomAvailableBuff();

        if (buff.GetBuffType() == BuffType.SHIELDED && m_ship.shielded)
            buff = buffDatabase.GetBuff(BuffType.OVERSHIELD);

        buffManager.AddBuff(buff, 5f, buff.GetBuffType() == BuffType.SHIELDED);
        buffBehaviour.Collected();
    }

    private void HandlePlayerGetHit(EnemyBehaviour enemyBehaviour)
    {
        if (enemyBehaviour == null) return;

        Enemy enemy = enemyBehaviour.GetEnemy();
        if (enemy == null) return;
        if (m_ship == null) return;

        if (!m_ship.shielded)
        {
            m_currentHP -= 1;
            OnHPChanged?.Invoke(m_currentHP);
            m_playerBehaviour.ApplyKnockback(enemyBehaviour.transform.position);
        }
        else
            buffManager.TryRemoveBuff(BuffType.SHIELDED);

        enemyBehaviour.Die();

        if (m_currentHP <= 0)
        {
            // game over
            inGameUIManager.GameOver(scoreManager.GetCurrentScore());
        }
    }

    private void HandleEnemyHit(EnemyBehaviour enemyBehaviour)
    {
        if (enemyBehaviour == null) return;

        Enemy enemy = enemyBehaviour.GetEnemy();
        if (enemy == null) return;

        scoreManager.AddScore(enemy);
        enemyBehaviour.Die();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (shipDatabase == null)
        {
            Debug.LogError("ShipDatabase is not assigned in GameRuleNew.");
            return;
        }

        // get player ship preference
        string shipPrefs = PlayerPrefs.GetString("SelectedShip", "Default");
        ShipEntry shipEntry = shipDatabase.GetShipByName(shipPrefs);
        if (shipEntry == null)
        {
            Debug.LogError("Selected ship not found in ShipDatabase.");
            return;
        }

        m_ship = shipEntry.ship;
        m_shipPrefab = shipEntry.shipPrefab;

        // spawns player ship at the center of camera view
        Vector3 spawnPosition = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        spawnPosition.z = 0; // set z to 0 for 2D

        GameObject shipObject = Instantiate(m_shipPrefab, spawnPosition, Quaternion.identity);

        m_playerBehaviour = shipObject.AddComponent<PlayerBehaviour>();

        if (m_ship == null || m_playerBehaviour == null)
        {
            Debug.LogError("Ship prefab does not have the required components.");
            return;
        }

        m_currentHP = m_ship.GetHP();
        m_playerBehaviour.Init(mainCamera, m_ship, movementBehaviour);
        inGameUIManager.Init(m_currentHP, 0);
        buffManager.Init(m_playerBehaviour);

        m_enemyMinSpeedConstraint = enemySpawnManager.baseMinEnemySpeed;
        m_enemyMaxSpeedConstraint = enemySpawnManager.baseMaxEnemySpeed;
        m_enemyMinSizeConstraint = enemySpawnManager.baseMinEnemySize;
        m_enemyMaxSizeConstraint = enemySpawnManager.baseMaxEnemySize;
        m_enemyMinSpawnCountConstraint = enemySpawnManager.baseMinSpawnCount;
        m_enemyMaxSpawnCountConstraint = enemySpawnManager.baseMaxSpawnCount;
        m_enemySpawnIntervalConstraint = enemySpawnManager.baseSpawnInterval;
    }
}
