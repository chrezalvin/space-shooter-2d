using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffScriptable : ScriptableObject
{
    public abstract BuffType GetBuffType();
    public abstract void ApplyBuff(ShipBehaviour shipBehaviour);
    public abstract void RemoveBuff(ShipBehaviour shipBehaviour);
}