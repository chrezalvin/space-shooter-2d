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

    public bool shielded = false;
    public bool isMultiShotActive = false;
    public bool isRapidShotActive = false;
    public bool isOverShieldActive = false;
    public bool isEvolved = false;

    public Ship(
        bool shielded = false,
        bool isMultiShotActive = false,
        bool isRapidShotActive = false,
        bool isOverShieldActive = false,
        bool isEvolved = false
    )
    {
        this.shielded = shielded;
        this.isMultiShotActive = isMultiShotActive;
        this.isRapidShotActive = isRapidShotActive;
        this.isOverShieldActive = isOverShieldActive;
        this.isEvolved = isEvolved;   
    }

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
