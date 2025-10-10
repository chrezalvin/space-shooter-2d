using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultBuffEvolution", menuName = "BuffEvolution/DefaultBuffEvolution")]
public class DefaultBuffEvolution : EvolutionSystem
{
    private BuffManager m_buffManager;
    private BuffDatabase m_buffDatabase;


    public override void Evolve(BuffManager buffManager, BuffDatabase buffDatabase)
    {
        m_buffManager = buffManager;
        m_buffDatabase = buffDatabase;
        BuffManager.OnBuffAdded += HandleBuffAdded;

        m_buffManager.AddBuff(m_buffDatabase.GetBuff(BuffType.DEFAULT_EVOLVE), 0f, true);
    }

    public override BuffType GetBuffType()
    {
        return BuffType.DEFAULT_EVOLVE;
    }

    private void HandleBuffAdded(ActiveBuff activeBuff)
    {
        if (activeBuff == null) return;

        activeBuff.totalDuration *= 2;
        activeBuff.remainingTime = activeBuff.totalDuration;
    }
}
