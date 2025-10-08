using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BuffSpawnManager : SpawnManager
{
    public GameObject buffPrefab;

    public float minBuffSpeed = 1f;
    public float maxBuffSpeed = 5f;

    public float spawnInterval = 5f;
    public int spawnChancePercent = 20;

    private float m_timer = 0f;

    // Update is called once per frame
    public override void Update()
    {
        m_timer += Time.deltaTime;
        if (m_timer >= spawnInterval)
        {
            m_timer = 0f;
            if (m_spawnedObjects.Count == 0 && Random.Range(0, 100) < spawnChancePercent)
            {
                SpawnBuff();
                Debug.Log("Buff spawned.");
            }
        }
        
        base.Update();
    }
    
    private GameObject SpawnBuff()
    {
        Vector2 from = GetRandomPointOnEdge(m_boundaryCollider.bounds);
        Vector2 to;

        do
        {
            to = GetRandomPointOnEdge(m_boundaryCollider.bounds);
        } while (Mathf.Approximately(from.x, to.x) || Mathf.Approximately(from.y, to.y)); // avoid vertical line

        GameObject buff = this.SpawnObject(buffPrefab, from, to);
        BuffBehaviour buffBehaviour = buff.GetComponent<BuffBehaviour>();
        if (buffBehaviour)
            buffBehaviour.Init(Random.Range(minBuffSpeed, maxBuffSpeed));
        else
            Debug.LogError("Buff prefab does not have a BuffBehaviour component.");

        return buff;
    }
}
