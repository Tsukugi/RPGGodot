public partial class UnitAttributes {
    int maxHitPoints = 1;
    int hitPoints = 1;

    public int Armor = 0;
    public int BaseDamage = 0;

    // Over 1 second (1/x);
    public double AttackSpeed = 1;
    public double AttackDuration = 0.3;

    public int HitPoints {
        get => hitPoints;
        set {
            // Try to avoid overflowing and underflowing
            if (value > maxHitPoints) hitPoints = maxHitPoints;
            else if (value < 0) hitPoints = 0;
            else hitPoints = value;
        }
    }

    public int MaxHitPoints { get => maxHitPoints; set => maxHitPoints = value; }

    public void ApplyDamage(int damage) {
        HitPoints -= damage - Armor;
    }

    public bool CanBeKilled() {
        return HitPoints <= 0;
    }

}