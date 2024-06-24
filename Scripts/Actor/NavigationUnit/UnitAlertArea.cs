
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

    public AlertState AlertState {
        get => alertState;
        set {
            alertState = value;
            unit.CombatArea.Target = null;
            GD.Print("[AlertState.Set] " + unit.Name + " -> " + alertState);
        }
    }

    public override void _Ready() {
        base._Ready();
        Unit parentUnit = this.TryFindParentNodeOfType<Unit>();
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

    void OnAlertAreaEntered(Node3D body) {
      
    }

    void OnAlertAreaExited(Node3D body) {
    }


    void AlertChangeOnEnemyUnitRange(NavigationUnit possibleEnemy) {
        if (unit is null) return;
        if (possibleEnemy.Attributes.CanBeKilled) return;
        if (!unit.Player.IsHostilePlayer(possibleEnemy.Player)) return;
        if (unit.UnitTask.CurrentTask != null && unit.UnitTask.CurrentTask.Type == TaskType.Attack) return;

        GD.Print("[UnitAlertArea.OnAlertAreaEntered] " + unit.Name + " -> " + possibleEnemy.Name);
        // TODO: Implement To ignore or hide or combat;
        OnCombatStarted(possibleEnemy);
    }

    void OnCombatStarted(NavigationUnit possibleEnemy) {
        alertState = AlertState.Combat;
        unit.CombatArea.Target = possibleEnemy;
        unit.UnitTask.ClearAll();
        unit.UnitTask.Add(new UnitTaskAttack(possibleEnemy, unit));
    }


    public void ToggleManualDraft() {
        if (alertState == AlertState.Combat) {
            alertState = AlertState.Safe;
        } else {
            alertState = AlertState.Combat;
        }
    }
}