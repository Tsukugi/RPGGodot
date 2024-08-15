public partial class UnitAttributes {
    protected readonly Unit unit;

    protected UnitAttributesDTO attributes = new();
    public bool CanBeKilled {
        get => attributes.hitPoints <= 0 || unit.IsQueuedForDeletion() || unit.IsKilled;
    }

    public UnitAttributes(Unit unit) {
        this.unit = unit;
    }

    public AttributesExport GetAttributes() {
        return new(attributes);
    }

    // * We only modify the base attributes on this method 
    public void InitializeValues(UnitAttributesDTO attributesDTO) {
        if (attributesDTO.attackSpeed <= 0) attributesDTO.attackSpeed = 0.1;
        attributes = attributesDTO;
        SetHitPoints(attributesDTO.maxHitPoints);
    }

    public int ApplyDamage(int damage) {
        int finalDamage = damage - attributes.armor;
        if (finalDamage < 0) finalDamage = 0;
        SetHitPoints(attributes.hitPoints - finalDamage);
        return finalDamage;
    }

    public int ApplyHeal(int healAmount) {
        SetHitPoints(attributes.hitPoints + healAmount);
        return healAmount;
    }

    // Try to avoid overflowing and underflowing
    protected void SetHitPoints(int newHitPoints) {
        if (newHitPoints > attributes.maxHitPoints) attributes.hitPoints = attributes.maxHitPoints;
        else if (newHitPoints < 0) attributes.hitPoints = 0;
        else attributes.hitPoints = newHitPoints;

        if (CanBeKilled) OnKilled(unit);
    }

    public delegate void OnKilledEventHandler(Unit unit);
    public event OnKilledEventHandler OnKilled;
}

public class AttributesExport {
    UnitAttributesDTO attributes;
    public AttributesExport(UnitAttributesDTO attributes) {
        this.attributes = attributes;
    }

    public int MaxHitPoints { get => attributes.maxHitPoints; }
    public double AttackSpeed { get => 1 / attributes.attackSpeed; }
    public int Armor { get => attributes.armor; }
    public int BaseDamage { get => attributes.baseDamage; }
    public double AttackCastDuration { get => attributes.attackCastDuration; }
    public float AttackRange { get => attributes.attackRange; }
    public float MovementSpeed { get => attributes.movementSpeed; }
    public int HitPoints { get => attributes.hitPoints; }
    public float AlertRange { get => attributes.alertRange; }
}