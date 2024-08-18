
using Godot;
public partial class VSEnemyUnit : VSUnit {

    VSGameManager gameManager;

    UnitCombatBase combatBase;

    public override void _Ready() {
        base._Ready();
        gameManager = this.TryFindParentNodeOfType<VSGameManager>();
        
        UnitAttributes.OnKilled += (dyingUnit) => RemoveUnit();
    }

    public override void _PhysicsProcess(double delta) {
        if (GlobalPosition.DistanceTo(gameManager.PlayerUnit.GlobalPosition) < 3) {
            MoveTowards(gameManager.PlayerUnit.GlobalPosition);
        } else {
            MoveTowards(GlobalPosition.WithZ(1000));
        }
        if (GlobalPosition.Z > 30) RemoveUnit();
    }
}
