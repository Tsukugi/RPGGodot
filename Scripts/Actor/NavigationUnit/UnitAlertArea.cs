using Godot;

public partial class UnitAlertArea : Area3D {
    NavigationUnit unit = null;
    CollisionShape3D collisionShape;
    AlertState alertState = AlertState.Safe;
    AlertState alertStateOnEnemySight = AlertState.Combat;
    public AlertState AlertState { get => alertState; }
    public AlertState AlertStateOnEnemySight { get => alertStateOnEnemySight; }

    public override void _Ready() {
        base._Ready();
        Unit parentUnit = this.TryFindParentNodeOfType<Unit>();
        if (parentUnit is NavigationUnit navigationUnit) {
            unit = navigationUnit;
            collisionShape = GetNode<CollisionShape3D>(StaticNodePaths.Area_CollisionShape);
            BodyEntered += OnAlertAreaEntered;
            Callable.From(() => {
                collisionShape.Scale = Vector3.One.Magnitude(unit.Attributes.AlertRange);
                SetMonitoringEnabled(true);
            }).CallDeferred();
        } else {
            SetMonitoringEnabled(false);
        }
    }

    public void EnableAlertAreaScan() {
        alertState = AlertState.Safe;
        SetMonitoringEnabled(true);
    }

    public void SetMonitoringEnabled(bool value) {
        if (unit is null) return;
        Callable.From(() => {
            Monitoring = value;
            collisionShape.Disabled = !value;
        }).CallDeferred();
    }

    void OnAlertAreaEntered(Node3D body) {
        if (unit is null) return;
        if (unit.IsKilled) return;
        if (body is not NavigationUnit possibleEnemy) return;
        if (possibleEnemy.IsKilled) return;
        if (!unit.Player.IsHostilePlayer(possibleEnemy.Player)) return;
        if (unit.UnitTask.CurrentTask != null && unit.UnitTask.CurrentTask.Type == TaskType.Attack) return;
        if (unit.UnitTask.CurrentTask != null && unit.UnitTask.CurrentTask.Type == TaskType.Hide) return;
        unit.Player.DebugLog("[UnitAlertArea.OnAlertAreaEntered] " + unit.Name + " -> " + possibleEnemy.Name);

        AlertChangeOnEnemyUnitRange(possibleEnemy);
    }

    void AlertChangeOnEnemyUnitRange(NavigationUnit possibleEnemy) {
        switch (alertStateOnEnemySight) {
            case AlertState.Hide:
                StartHideTask(possibleEnemy);
                break;
            case AlertState.Combat:
                StartCombatTask(possibleEnemy);
                break;
        }
        SetMonitoringEnabled(false);
    }

    void StartHideTask(NavigationUnit possibleEnemy) {
        alertState = AlertState.Hide;
        UnitTaskHide newHideTask = new(possibleEnemy, unit);
        void onTaskCompletedEvent(TaskBase task) { OnAlertEnd(); }
        newHideTask.OnTaskCompletedEvent -= onTaskCompletedEvent;
        newHideTask.OnTaskCompletedEvent += onTaskCompletedEvent;
        unit.UnitTask.PriorityAddTask(newHideTask);
    }

    void StartCombatTask(NavigationUnit possibleEnemy) {
        alertState = AlertState.Combat;
        unit.UnitCombat.StartCombatTask(possibleEnemy);
        unit.UnitCombat.OnCombatEndEvent -= OnAlertEnd;
        unit.UnitCombat.OnCombatEndEvent += OnAlertEnd;
    }

    void OnAlertEnd() {
        EnableAlertAreaScan();
        unit.Player.DebugLog(unit.Name + " -> [UnitAlertArea.OnAlertEnd] Finished correctly");
    }

    void ToggleManualDraft() {
        if (alertState == AlertState.Combat) {
            alertState = AlertState.Safe;
        } else {
            alertState = AlertState.Combat;
        }
    }
}
public enum AlertState {
    Safe,
    Hide,
    Combat,
}
