using UnityEngine;

[CreateAssetMenu(fileName = "OverShieldBuff", menuName = "Game/Buffs/OverShieldBuff")]
public class OverShieldBuff : BuffScriptable
{
    public override BuffType GetBuffType()
    {
        return BuffType.OVERSHIELD;
    }

    public override void ApplyBuff(ShipBehaviour shipBehaviour)
    {
        shipBehaviour.SetOverShield(true);
    }

    public override void RemoveBuff(ShipBehaviour shipBehaviour)
    {
        shipBehaviour.SetOverShield(false);
    }
}
