using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineShipBehaviour : ShipBehaviour
{
    public EvolutionSystem evolutionSystem;
    public override void ApplyEvolve(BuffManager buffManager, BuffDatabase buffDatabase)
    {
        evolutionSystem.Evolve(buffManager, buffDatabase);
        m_ship.isEvolved = true;
    }
}
