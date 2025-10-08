using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HornetBuffEvolution", menuName = "BuffEvolution/HornetBuffEvolution")]
public class HornetBuffEvolution : EvolutionSystem
{
    private BuffManager m_buffManager;
    private BuffDatabase m_buffDatabase;

    // Track buffs added via evolution to avoid recursion
    private static bool s_isHandlingBuff = false;

    public override void Evolve(BuffManager buffManager, BuffDatabase buffDatabase)
    {
        m_buffManager = buffManager;
        m_buffDatabase = buffDatabase;
        BuffManager.OnBuffAdded += HandleBuffAddedHornet;

        m_buffManager.AddBuff(m_buffDatabase.GetBuff(BuffType.HORNET_EVOLVE), 0f, true);
    }

    public override BuffType GetBuffType()
    {
        return BuffType.HORNET_EVOLVE;
    }

    private void HandleBuffAddedHornet(ActiveBuff activeBuff)
    {
        if (activeBuff == null) return;

        // Ignore if the buff added is from this evolution's side-effect
        if (s_isHandlingBuff) return;

        s_isHandlingBuff = true;

        // Add the additional evolution buff
        m_buffManager.AddBuff(m_buffDatabase.GetBuff(BuffType.RAPID_SHOT), 5f);

        s_isHandlingBuff = false;
    }
}
