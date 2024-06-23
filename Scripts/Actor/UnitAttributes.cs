using Godot;

public partial class UnitAttributes : Node {
    Unit unit = null;
    [Export]
    float movementSpeed = 3f;
    [Export]
    int maxHitPoints = 10;
    [Export]
    int hitPoints = 10;
    [Export]
    double attackSpeed = 1;
    [Export]
    int armor = 1;
    [Export]
    int baseDamage = 5;
    [Export]
    double attackDuration = 0.3;
    [Export]
    double attackRange = 2;

    public bool CanBeKilled {
        get => hitPoints <= 0;
    }

    public int HitPoints { get => hitPoints; }
    public int MaxHitPoints { get => maxHitPoints; }
    public double AttackSpeed { get => 1 / attackSpeed; }
    public int Armor { get => armor; }
    public int BaseDamage { get => baseDamage; }
    public double AttackDuration { get => attackDuration; }
    public double AttackRange { get => attackRange; }
    public float MovementSpeed { get => movementSpeed; }

    public override void _Ready() {
        base._Ready();
        unit = this.TryFindUnit();
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        // ? Unit requires to not be visible and CanBeKilled to be deleted
        if (CanBeKilled) unit.Visible = false;
    }

    // Try to avoid overflowing and underflowing
    void SetHitPoints(int newHitPoints) {
        if (newHitPoints > maxHitPoints) hitPoints = maxHitPoints;
        else if (newHitPoints < 0) hitPoints = 0;
        else hitPoints = newHitPoints;
    }


    public void Update(int maxHitPoints, int armor, int baseDamage) {
        this.maxHitPoints = maxHitPoints;
        this.armor = armor;
        this.baseDamage = baseDamage;
        SetHitPoints(maxHitPoints);

    }

    public void ApplyDamage(int damage) {
        int finalDamage = damage - Armor;
        if (finalDamage < 0) finalDamage = 0;
        GD.Print("[ApplyDamage] Actor dealt " + finalDamage + " of damage. Target hitpoints: " + (hitPoints - finalDamage));
        SetHitPoints(hitPoints - finalDamage);
    }

}