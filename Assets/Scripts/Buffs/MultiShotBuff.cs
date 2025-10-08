using UnityEngine;

[CreateAssetMenu(fileName = "MultiShotBuff", menuName = "Game/Buffs/MultiShotBuff")]
public class MultiShotBuff : BuffScriptable
{
    public override BuffType GetBuffType()
    {
        return BuffType.MULTI_SHOT;
    }

    public override void ApplyBuff(ShipBehaviour shipBehaviour)
    {
        shipBehaviour.ApplyMultiShot(true);
    }

    public override void RemoveBuff(ShipBehaviour shipBehaviour)
    {
        shipBehaviour.ApplyMultiShot(false);
    }
}
