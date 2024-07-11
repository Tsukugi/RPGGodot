using Godot;

public partial class UnitAttributes : Node {
    Unit unit;
    [Export]
    float movementSpeed = 3f;
    [Export]
    int maxHitPoints = 50;
    [Export]
    int hitPoints = 50;
    [Export]
    double attackSpeed = 0.5;
    [Export]
    int armor = 1;
    [Export]
    int baseDamage = 20;
    [Export]
    double attackCastDuration = 0.3;
    [Export]
    float attackRange = 10;

    public bool CanBeKilled {
        get => hitPoints <= 0 || unit.IsQueuedForDeletion();
    }

    public int HitPoints { get => hitPoints; }
    public int MaxHitPoints { get => maxHitPoints; }
    public double AttackSpeed { get => 1 / attackSpeed; }
    public int Armor { get => armor; }
    public int BaseDamage { get => baseDamage; }
    public double AttackCastDuration { get => attackCastDuration; }
    public float AttackRange { get => attackRange; }
    public float MovementSpeed { get => movementSpeed; }

    public override void _Ready() {
        base._Ready();
        unit = this.TryFindParentNodeOfType<Unit>();
    }

    // Try to avoid overflowing and underflowing
    void SetHitPoints(int newHitPoints) {
        if (newHitPoints > maxHitPoints) hitPoints = maxHitPoints;
        else if (newHitPoints < 0) hitPoints = 0;
        else hitPoints = newHitPoints;

        if (CanBeKilled) OnKilled(unit);
    }

    public void Update(NavigationUnitAttributesDTO attributesDTO) {
        maxHitPoints = attributesDTO.maxHitPoints;
        armor = attributesDTO.armor;
        baseDamage = attributesDTO.baseDamage;
        movementSpeed = attributesDTO.movementSpeed;
        attackSpeed = attributesDTO.attackSpeed <= 0 ? 0.1 : attributesDTO.attackSpeed;
        attackCastDuration = attributesDTO.attackCastDuration;
        attackRange = attributesDTO.attackRange;
        SetHitPoints(maxHitPoints);
    }

    public int ApplyDamage(int damage) {
        int finalDamage = damage - Armor;
        if (finalDamage < 0) finalDamage = 0;
        SetHitPoints(hitPoints - finalDamage);
        return finalDamage;
    }

    public delegate void OnKilledEventHandler(Unit unit);
    public event OnKilledEventHandler OnKilled;

}