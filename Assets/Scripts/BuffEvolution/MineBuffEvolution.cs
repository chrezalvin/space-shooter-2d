using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MineBuffEvolution", menuName = "BuffEvolution/MineBuffEvolution")]
public class MineBuffEvolution : EvolutionSystem
{
    private BuffManager m_buffManager;
    private BuffDatabase m_buffDatabase;

    // to prevent recursion
    private static bool s_isHandlingBuff = false;

    public override void Evolve(BuffManager buffManager, BuffDatabase buffDatabase)
    {
        m_buffManager = buffManager;
        m_buffDatabase = buffDatabase;

        BuffManager.OnBuffAdded += HandleBuffAddedMine;

        m_buffManager.AddBuff(m_buffDatabase.GetBuff(BuffType.MINE_EVOLVE), 0f, true);
    }

    public override BuffType GetBuffType()
    {
        return BuffType.MINE_EVOLVE;
    }

    private void HandleBuffAddedMine(ActiveBuff activeBuff)
    {
        if (activeBuff == null) return;

        if (s_isHandlingBuff) return;

        s_isHandlingBuff = true;

        BuffType buffType = activeBuff.buff.GetBuffType();
        // implies that the buff added is SHIELDED
        if (buffType == BuffType.SHIELDED || buffType == BuffType.OVERSHIELD)
        {
            // apply temporary OVERSHIELD if SHIELDED is added or permanent OVERSHIELD if OVERSHIELD is added
            m_buffManager.AddBuff(m_buffDatabase.GetBuff(BuffType.OVERSHIELD), 5f, buffType == BuffType.OVERSHIELD);
        }

        s_isHandlingBuff = false;
    }
}
