using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EvolutionSystem: MonoBehaviour
{
    public abstract BuffType GetBuffType();
    public abstract void Evolve(BuffManager buffManager, BuffDatabase buffDatabase);

    public abstract void OnDestroy();
    public abstract void OnDisable();
}