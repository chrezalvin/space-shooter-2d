using UnityEngine;

[CreateAssetMenu(fileName = "RapidShotBuff", menuName = "Game/Buffs/RapidShotBuff")]
public class RapidShotBuff : BuffScriptable
{
    public override BuffType GetBuffType()
    {
        return BuffType.RAPID_SHOT;
    }

    public override void ApplyBuff(ShipBehaviour shipBehaviour)
    {
        shipBehaviour.ApplyRapidShot(true);
    }

    public override void RemoveBuff(ShipBehaviour shipBehaviour)
    {
        shipBehaviour.ApplyRapidShot(false);
    }
}
