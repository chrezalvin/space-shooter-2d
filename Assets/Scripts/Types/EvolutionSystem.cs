using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EvolutionSystem : ScriptableObject
{
    public abstract BuffType GetBuffType();
    public abstract void Evolve(BuffManager buffManager, BuffDatabase buffDatabase);
}

// public abstract class EvolutionSystem : ScriptableObject
// {
//     private BuffManager m_buffManager;
//     private BuffDatabase m_buffDatabase;

//     public abstract void Evolve(BuffManager buffManager, BuffDatabase buffDatabase)
//     {
//         m_buffManager = buffManager;
//         m_buffDatabase = buffDatabase;

//         BuffManager.OnBuffAdded += HandleBuffAddedHornet;
//     }

//     private void HandleBuffAddedMine(ActiveBuff activeBuff)
//     {
//         if (activeBuff == null) return;

//         if (activeBuff.buff.GetBuffType() == BuffType.SHIELDED)
//         {
//             // check if player already have shielded buff
//             m_buffManager.AddBuff(m_buffDatabase.GetBuff(BuffType.OVERSHIELD), 5f, m_buffManager.HasBuff(BuffType.SHIELDED));
//         }
//     }

//     private void HandleBuffAddedHornet(ActiveBuff activeBuff)
//     {
//         if (activeBuff == null) return;

//         m_buffManager.AddBuff(m_buffDatabase.GetBuff(BuffType.RAPID_SHOT), 5f);
//     }
// }
