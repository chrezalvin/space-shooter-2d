using UnityEngine;

[CreateAssetMenu(fileName = "NewShip", menuName = "Game/Ship")]
public class Ship : ScriptableObject
{
    [SerializeField]
    protected int hp = 5;

    [SerializeField]
    protected float attackSpeed = 0.5f;

    [SerializeField]
    protected float movementSpeed = 10f;

    [SerializeField]
    protected string shipName = "Unnamed Ship";

    [SerializeField, TextArea]
    protected string description = "No description";

    [SerializeField]
    protected Buff evolveBuff = null;

    [HideInInspector] public bool shielded = false;
    [HideInInspector] public bool isMultiShotActive = false;
    [HideInInspector] public bool isRapidShotActive = false;
    [HideInInspector] public bool isOverShieldActive = false;
    [HideInInspector] public bool isEvolved = false;
    
    // getter
    public int GetHP() { return hp; }
    public float GetAttackSpeed()
    {
        if (isRapidShotActive)
            return attackSpeed / 2f;
        else
            return attackSpeed;
    }
    public float GetMovementSpeed() { return movementSpeed; }
    public string GetShipName() { return shipName; }
    public string GetDescription() { return description; }

    // setter
    public int GetsHit()
    {
        --hp;
        return hp;
    }

    public Buff GetEvolveBuff() { return evolveBuff; }
}
