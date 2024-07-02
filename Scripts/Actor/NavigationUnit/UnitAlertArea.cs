
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

    public AlertState AlertState { get => alertState; }

    public override void _Ready() {
        base._Ready();
        Unit parentUnit = this.TryFindParentNodeOfType<Unit>();
        if (parentUnit is NavigationUnit navigationUnit) {
            unit = navigationUnit;
            collisionShape = GetNodeOrNull<CollisionShape3D>(StaticNodePaths.Area_CollisionShape);
            Callable.From(InitializeDeferred).CallDeferred();
        } else {
            GD.Print("[UnitAlertArea._Ready] " + parentUnit.Name + " has no NavigationUnit as parent, removing this Area as it is not needed.");
            QueueFree();
        }
    }

    void InitializeDeferred() {
        Monitoring = true;
        collisionShape.Disabled = false;
        collisionShape.Scale = VectorUtils.Magnitude(unit.Attributes.AttackRange + 3f);
        BodyEntered += OnAlertAreaEntered;
    }

    void OnCombatEnd() {
        if (alertState == AlertState.Combat) alertState = AlertState.Safe;
        SetDeferred("Monitoring", true);
    }

    void OnAlertAreaEntered(Node3D body) {
        if (unit is null) return;
        if (unit.UnitSelection.IsSelected) return;
        if (body is not NavigationUnit navigationUnit) return;
        AlertChangeOnEnemyUnitRange(navigationUnit);
    }

    void AlertChangeOnEnemyUnitRange(NavigationUnit possibleEnemy) {
        if (unit is null) return;
        if (possibleEnemy.Attributes.CanBeKilled) return;
        if (!unit.Player.IsHostilePlayer(possibleEnemy.Player)) return;
        if (unit.UnitTask.CurrentTask != null && unit.UnitTask.CurrentTask.Type == TaskType.Attack) return;

        unit.Player.DebugLog("[UnitAlertArea.OnAlertAreaEntered] " + unit.Name + " -> " + possibleEnemy.Name);
        // TODO: Implement To ignore or hide or combat;
        StartCombatTask(possibleEnemy);
        SetDeferred("Monitoring", false);
    }

    void StartCombatTask(NavigationUnit possibleEnemy) {
        alertState = AlertState.Combat;
        unit.UnitCombat.StartCombatTask(possibleEnemy);
        unit.UnitCombat.OnCombatEndEvent += OnCombatEnd;
    }

    public void CalmDown() {
        alertState = AlertState.Safe;
    }


    public void ToggleManualDraft() {
        if (alertState == AlertState.Combat) {
            alertState = AlertState.Safe;
        } else {
            alertState = AlertState.Combat;
        }
    }
}