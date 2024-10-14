using System.Collections.Generic;

public partial class UnitAttributes {
    protected readonly Unit unit;

    protected UnitAttributesDTO attributes = new();

    public bool CanBeKilled {
        get => attributes.hitPoints <= 0 || unit.IsQueuedForDeletion() || unit.IsKilled;
    }

    public UnitAttributes(Unit unit) {
        this.unit = unit;
    }

    public Dictionary<string, object> GetAttributesAsDictionary() {
        return attributes.GetObjectFields();
    }

    public AttributesExport GetAttributes() {
        return new(attributes);
    }

    // * We only modify the base attributes on this method 
    public void InitializeValues(UnitAttributesDTO attributesDTO) {
        if (attributesDTO.attackSpeed <= 0) attributesDTO.attackSpeed = 0.1f;
        attributes = attributesDTO;
        SetHitPoints(attributesDTO.maxHitPoints);
    }

    public double ApplyDamage(double damage) {
        var finalDamage = damage - attributes.armor;
        if (finalDamage < 0) finalDamage = 0;
        SetHitPoints(attributes.hitPoints - finalDamage);
        return finalDamage;
    }

    public int ApplyHeal(int healAmount) {
        SetHitPoints(attributes.hitPoints + healAmount);
        return healAmount;
    }

    // Try to avoid overflowing and underflowing
    protected void SetHitPoints(double newHitPoints) {
        if (newHitPoints > attributes.maxHitPoints) attributes.hitPoints = attributes.maxHitPoints;
        else if (newHitPoints < 0) attributes.hitPoints = 0;
        else attributes.hitPoints = newHitPoints;

        if (CanBeKilled) OnKilled(unit);
    }

    public void SetMaxHitPoints(int newHitPoints) {
        if (newHitPoints <= 0) attributes.maxHitPoints = 1;
        else attributes.maxHitPoints = newHitPoints;
        SetHitPoints(newHitPoints);
    }

    public delegate void OnKilledEventHandler(Unit unit);
    public event OnKilledEventHandler OnKilled;
}

public class AttributesExport {
    readonly UnitAttributesDTO attributes = new() { attackSpeed = 1f };
    public AttributesExport() {
    }
    public AttributesExport(UnitAttributesDTO attributesDTO) {
        attributes = attributesDTO;
    }

    public int MaxHitPoints { get => (int)attributes.maxHitPoints; }
    public double AttackSpeed { get => 1 / attributes.attackSpeed; }
    public int Armor { get => (int)attributes.armor; }
    public int BaseDamage { get => (int)attributes.baseDamage; }
    public double AttackCastDuration { get => attributes.attackCastDuration; }
    public float AttackRange { get => (float)attributes.attackRange; }
    public float MovementSpeed { get => (float)attributes.movementSpeed; }
    public int HitPoints { get => (int)attributes.hitPoints; }
    public float AlertRange { get => (float)attributes.alertRange; }
}