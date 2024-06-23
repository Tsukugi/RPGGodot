
using System.Collections.Generic;
using Godot;

public enum AlertState {
    Safe,
    Hide,
    Combat,
}

public partial class UnitAlertArea : Area3D {
    NavigationUnit unit = null;
    CollisionShape3D collisionShape;
    AlertState alertState = AlertState.Safe;

    public AlertState AlertState { get => alertState; set => alertState = value; }

    public override void _Ready() {
        base._Ready();
        Unit parentUnit = this.FindUnitNode();
        if (parentUnit is NavigationUnit navigationUnit) {
            unit = navigationUnit;
            collisionShape = GetNodeOrNull<CollisionShape3D>(StaticNodePaths.Area_CollisionShape);
            collisionShape.Disabled = false;
            BodyEntered += OnAlertAreaEntered;
            BodyExited += OnAlertAreaExited;
        } else {
            GD.Print("[UnitAlertArea._Ready] " + parentUnit.Name + " has no NavigationUnit as parent, removing this Area as it is not needed.");
            QueueFree();
        }
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (unit is not NavigationUnit) return;
        if (unit.UnitSelection.IsSelected) return;
        // TODO: I think is better to optimize this with a live dictionary, instead of calling this every physicsFrame.
        List<NavigationUnit> bodies = GetOverlappingBodies().FilterNavigationUnits();
        foreach (var body in bodies) {
            AlertChangeOnEnemyUnitRange(body);
        }

    }

    void OnAlertAreaEntered(Node3D body) {
        if (unit is not NavigationUnit) return;
        if (unit.UnitSelection.IsSelected) return;
        if (body is not NavigationUnit navigationUnit) return;
        AlertChangeOnEnemyUnitRange(navigationUnit);
    }

    void OnAlertAreaExited(Node3D body) {
        if (unit is not NavigationUnit) return;
        if (unit.UnitSelection.IsSelected) return;
    }


    void AlertChangeOnEnemyUnitRange(NavigationUnit possibleEnemy) {
        if (unit is not NavigationUnit) return;
        if (!possibleEnemy.Player.IsHostile()) return;
        // TODO: Implement To ignore or hide or combat;
        alertState = AlertState.Combat;
       // unit.CombatArea.Target = possibleEnemy;
    }


    public void ToggleManualDraft() {
        if (alertState == AlertState.Combat) {
            alertState = AlertState.Safe;
        } else {
            alertState = AlertState.Combat;
        }
    }
}