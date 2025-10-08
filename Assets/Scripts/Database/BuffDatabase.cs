using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffDatabase", menuName = "Game/Buff Database")]
public class BuffDatabase : ScriptableObject
{
    [SerializeField] public List<Buff> allBuffs;

    public List<Buff> AllBuffs()
    {
        return allBuffs;
    }

    public List<Buff> AllBuffs(BuffStatus status)
    {
        List<Buff> filteredBuffs = new List<Buff>(allBuffs);
        filteredBuffs.RemoveAll(buff => buff.GetBuffStatus() != status);
        return filteredBuffs;
    }

    public Buff GetRandomAvailableBuff()
    {
        List<Buff> availableBuffs = AllBuffs(BuffStatus.BUFF_AVAILABLE);
        Buff buff = availableBuffs[Random.Range(0, availableBuffs.Count)];
        return CopyBuff(buff);
    }

    public Buff GetRandomBuff(List<BuffType> excludeTypes = null)
    {
        List<Buff> filteredBuffs = new List<Buff>(allBuffs);
        if (excludeTypes != null && excludeTypes.Count > 0)
            filteredBuffs.RemoveAll(buff => excludeTypes.Contains(buff.GetBuffType()));

        Buff buff = filteredBuffs[Random.Range(0, filteredBuffs.Count)];

        return CopyBuff(buff);
    }

    public Buff GetBuff(BuffType type)
    {
        foreach (Buff buff in allBuffs)
            if (buff.GetBuffType() == type)
                return CopyBuff(buff);

        return null;
    }

    public Buff CopyBuff(Buff buff)
    {
        Buff newBuff = Instantiate(buff);
        return newBuff;
    }

    public static Buff GetBuffStatic()
    {
        return null;   
    }
}
