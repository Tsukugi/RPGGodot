
public partial class CharacterAttributes {
    int maxHitPoints = 1;
    public int Armor = 0;
    public int BaseDamage = 0;
    int hitPoints = 1;

    public int MaxHitPoints {
        get => maxHitPoints;
        set {
            maxHitPoints = value;
        }
    }

    public int HitPoints {
        get => hitPoints;
        set {
            if (hitPoints >= maxHitPoints) hitPoints = maxHitPoints;
            else hitPoints = value;
        }
    }

    public void ApplyDamage(int damage) {
        HitPoints -= damage - Armor;
    }

    public bool CanBeKilled() {
        return HitPoints <= 0;
    }

}