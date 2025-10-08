using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRule : MonoBehaviour
{
    public static Action<int> OnHPChanged;
    public static Action<int> OnScoreChanged;

    public int minScoreToEvolve = 50;

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

    private void OnEnable()
    {
        BulletBehaviour.OnEnemyHit += HandleEnemyHit;
        PlayerBehaviour.OnPlayerHit += HandlePlayerGetHit;
        PlayerBehaviour.OnBuffCollected += HandleBuffCollected;
    }

    private void OnDisable()
    {
        BulletBehaviour.OnEnemyHit -= HandleEnemyHit;
        PlayerBehaviour.OnPlayerHit -= HandlePlayerGetHit;
        PlayerBehaviour.OnBuffCollected -= HandleBuffCollected;
    }

    private void HandleBuffCollected(BuffBehaviour buffBehaviour)
    {
        if (buffBehaviour == null) return;

        // get buffs randomly
        // check if player have shield buff
        Buff buff = buffDatabase.GetRandomAvailableBuff();
        
        if(buff.GetBuffType() == BuffType.SHIELDED && m_ship.shielded)
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
            inGameUIManager.UpdateHPBar(m_currentHP);
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

        int score = scoreManager.GetCurrentScore();

        if (score > minScoreToEvolve && !m_ship.isEvolved)
            m_playerBehaviour.GetShipBehaviour().ApplyEvolve(buffManager, buffDatabase);

        inGameUIManager.UpdateScore(score);
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

        // attach PlayerBehaviour to shipObject
        m_playerBehaviour = shipObject.AddComponent<PlayerBehaviour>();

        if (m_ship == null || m_playerBehaviour == null)
        {
            Debug.LogError("Ship prefab does not have the required components.");
            return;
        }

        m_currentHP = m_ship.GetHP();
        m_playerBehaviour.Init(mainCamera, m_ship);
        inGameUIManager.Init(m_currentHP, 0);
        buffManager.Init(m_playerBehaviour);
    }
}
