using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static event Action<ActiveBuff> OnBuffWillAdded;
    public static event Action<ActiveBuff> OnBuffAdded;
    public static event Action<ActiveBuff> OnBuffRemoved;

    private Dictionary<BuffType, ActiveBuff> m_activeBuffs;
    private PlayerBehaviour m_playerBehaviour;

    // Start is called before the first frame update
    public void Init(PlayerBehaviour playerBehaviour)
    {
        m_playerBehaviour = playerBehaviour;
        m_activeBuffs = new Dictionary<BuffType, ActiveBuff>();
    }

    // Update is called once per frame
    void Update()
    {
        List<BuffType> buffToRemove = new List<BuffType>();

        // don't update the variable in this loop
        foreach (var buff in m_activeBuffs.ToList())
        {
            if (buff.Value.ignoreTimer) continue;

            buff.Value.remainingTime -= Time.deltaTime;
            if (buff.Value.remainingTime <= 0f)
                buffToRemove.Add(buff.Key);
        }

        foreach (var bt in buffToRemove)
            TryRemoveBuff(bt);
    }
    
    public ActiveBuff AddBuff(Buff buff, float duration, bool ignoreTimer = false)
    {
        if (buff == null) return null;

        ActiveBuff ab = new ActiveBuff(buff, duration, ignoreTimer);

        OnBuffWillAdded?.Invoke(ab);
        ab.buff.ApplyBuff(m_playerBehaviour.GetShipBehaviour());

        m_activeBuffs[buff.GetBuffType()] = ab;

        OnBuffAdded?.Invoke(ab);

        if (ab.ignoreTimer)
            Debug.Log($"Added buff: {buff.GetBuffType()}.");
        else
            Debug.Log($"Added buff: {buff.GetBuffType()} for {duration} seconds.");

        return ab;
    }

    public bool TryRemoveBuff(BuffType buffType)
    {
        if (buffType == BuffType.SHIELDED && m_activeBuffs.ContainsKey(BuffType.OVERSHIELD))
        {
            ActiveBuff ab = m_activeBuffs[BuffType.OVERSHIELD];
            ab.buff.RemoveBuff(m_playerBehaviour.GetShipBehaviour());
            m_activeBuffs.Remove(BuffType.OVERSHIELD);

            OnBuffRemoved?.Invoke(ab);
            Debug.Log($"Removed buff: {BuffType.OVERSHIELD}");

            return true;
        }
        else if (m_activeBuffs.ContainsKey(buffType))
        {
            ActiveBuff ab = m_activeBuffs[buffType];
            ab.buff.RemoveBuff(m_playerBehaviour.GetShipBehaviour());
            m_activeBuffs.Remove(buffType);

            OnBuffRemoved?.Invoke(ab);
            Debug.Log($"Removed buff: {buffType}");
            return true;
        }
        
        return false;
    }

    public bool HasBuff(BuffType buffType)
    {
        return m_activeBuffs.ContainsKey(buffType);
    }

    // getter for activeBuffs
    public Dictionary<BuffType, ActiveBuff> GetActiveBuffs()
    {
        return m_activeBuffs;
    }
}
