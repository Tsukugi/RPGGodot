using Godot;
public partial class UnitCombatArea : Area3D {
    NavigationUnit unit = null;
    Unit target = null;
    public Unit Target {
        get => target; set {
            target = value;
            unit.UnitTask.Add(new UnitTaskAttack(target, unit));
        }
    }

    public override void _Ready() {
        base._Ready();
        Unit parentUnit = this.FindUnitNode();
        if (parentUnit is NavigationUnit navigationUnit) {
            unit = navigationUnit;
            /*   collisionShape = GetNodeOrNull<CollisionShape3D>(StaticNodePaths.Area_CollisionShape);
               collisionShape.Disabled = false;
               BodyEntered += OnAlertAreaEntered;
               BodyExited += OnAlertAreaExited;*/
        } else {
            GD.Print("[UnitAlertArea._Ready] " + parentUnit.Name + " has no NavigationUnit as parent, removing this Area as it is not needed.");
            QueueFree();
        }
    }
}