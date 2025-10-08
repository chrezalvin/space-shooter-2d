using UnityEngine;

[CreateAssetMenu(fileName = "ShieldBuff", menuName = "Game/Buffs/ShieldBuff")]
public class ShieldBuff : BuffScriptable
{
    public override BuffType GetBuffType()
    {
        return BuffType.SHIELDED;
    }

    public override void ApplyBuff(ShipBehaviour shipBehaviour)
    {
        shipBehaviour.SetShield(true);
    }

    public override void RemoveBuff(ShipBehaviour shipBehaviour)
    {
        shipBehaviour.SetShield(false);
    }
}
