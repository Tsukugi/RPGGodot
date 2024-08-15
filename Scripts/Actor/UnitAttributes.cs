using Godot;

public partial class UnitAttributes : Node {
    Unit unit;

    protected UnitAttributesDTO attributes = new() {
        movementSpeed = 3f,
        maxHitPoints = 50,
        attackSpeed = 0.5,
        armor = 1,
        baseDamage = 20,
        attackCastDuration = 0.3,
        attackRange = 10,
    };

    readonly InternalAttributesDTO internalAttributes = new() {
        hitPoints = 50,
        alertRange = 7f,
    };

    public bool CanBeKilled {
        get => internalAttributes.hitPoints <= 0 || unit.IsQueuedForDeletion() || unit.IsKilled;
    }

    public int HitPoints { get => internalAttributes.hitPoints; }
    public int MaxHitPoints { get => attributes.maxHitPoints; }
    public double AttackSpeed { get => 1 / attributes.attackSpeed; }
    public int Armor { get => attributes.armor; }
    public int BaseDamage { get => attributes.baseDamage; }
    public double AttackCastDuration { get => attributes.attackCastDuration; }
    public float AttackRange { get => attributes.attackRange; }
    public float MovementSpeed { get => attributes.movementSpeed; }
    public float AlertRange { get => internalAttributes.alertRange; }

    public override void _Ready() {
        base._Ready();
        unit = this.TryFindParentNodeOfType<Unit>();
    }

    // Try to avoid overflowing and underflowing
    protected void SetHitPoints(int newHitPoints) {
        if (newHitPoints > attributes.maxHitPoints) internalAttributes.hitPoints = attributes.maxHitPoints;
        else if (newHitPoints < 0) internalAttributes.hitPoints = 0;
        else internalAttributes.hitPoints = newHitPoints;

        if (CanBeKilled) OnKilled(unit);
    }

    public void UpdateValues(UnitAttributesDTO attributesDTO) {
        if (attributesDTO.attackSpeed <= 0) attributesDTO.attackSpeed = 0.1;
        attributes = attributesDTO;
        SetHitPoints(attributes.maxHitPoints);
    }


    public int ApplyDamage(int damage) {
        int finalDamage = damage - Armor;
        if (finalDamage < 0) finalDamage = 0;
        SetHitPoints(internalAttributes.hitPoints - finalDamage);
        return finalDamage;
    }

    public int ApplyHeal(int healAmount) {
        SetHitPoints(internalAttributes.hitPoints + healAmount);
        return healAmount;
    }

    public delegate void OnKilledEventHandler(Unit unit);
    public event OnKilledEventHandler OnKilled;

}

public class InternalAttributesDTO {
    public int hitPoints;
    public float alertRange;
}