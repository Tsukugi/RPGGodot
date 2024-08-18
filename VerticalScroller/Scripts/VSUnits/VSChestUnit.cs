using Godot;
public partial class VSChestUnit : VSUnit {
    public override void _Ready() {
        base._Ready();
        UnitAttributes.OnKilled += (dyingUnit) => RemoveUnit();
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        MoveTowards(GlobalPosition.WithZ(1000));
        if (GlobalPosition.Z > 30) RemoveUnit();
    }
}