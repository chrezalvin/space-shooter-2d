using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "Game/Buff")]
public class Buff : ScriptableObject
{

    [SerializeField]
    protected BuffType buffType;

    [SerializeField]
    protected BuffStatus buffStatus;

    [SerializeField]
    protected Sprite icon;

    [SerializeField]
    protected string buffName;

    [SerializeField, TextArea(3, 10)]
    protected string description;

    [SerializeField]
    protected BuffScriptable buffScriptable;

    public BuffType GetBuffType()
    {
        return buffType;
    }

    public string GetBuffName()
    {
        return buffName;
    }

    public string GetDescription()
    {
        return description;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public void ApplyBuff(ShipBehaviour shipBehaviour)
    {
        buffScriptable?.ApplyBuff(shipBehaviour);
    }

    public void RemoveBuff(ShipBehaviour shipBehaviour)
    {
        buffScriptable.RemoveBuff(shipBehaviour);
    }

    public BuffStatus GetBuffStatus()
    {
        return buffStatus;
    }
}
