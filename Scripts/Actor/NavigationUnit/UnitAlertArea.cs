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

    public float AreaRadius { get => collisionShape.Scale.X; }

    public AlertState AlertStateOnEnemySight = AlertState.Combat;
    public AlertState AlertState { get => alertState; }

    public override void _Ready() {
        base._Ready();
        Unit parentUnit = this.TryFindParentNodeOfType<Unit>();
        if (parentUnit is NavigationUnit navigationUnit) {
            unit = navigationUnit;
            collisionShape = GetNodeOrNull<CollisionShape3D>(StaticNodePaths.Area_CollisionShape);
            Callable.From(InitializeDeferred).CallDeferred();
        } else {
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
        CalmDown();
        unit.Player.DebugLog(unit.Name + " -> [StartAttackTask -> OnCombatEndEvent : OnCombatEnd] Finished correctly");
        SetDeferred("Monitoring", true);
    }

    void OnHideEnd(TaskBase task) {
        CalmDown();
        unit.Player.DebugLog(unit.Name + " -> [StartHideTask -> OnTaskCompletedEvent : OnHideEnd] Finished correctly");
        SetDeferred("Monitoring", true);
    }

    void OnAlertAreaEntered(Node3D body) {
        if (unit is null) return;
        //if (unit.UnitSelection.IsSelected) return;
        if (body is not NavigationUnit navigationUnit) return;
        AlertChangeOnEnemyUnitRange(navigationUnit);
    }

    void AlertChangeOnEnemyUnitRange(NavigationUnit possibleEnemy) {
        if (unit is null) return;
        if (possibleEnemy.Attributes.CanBeKilled) return;
        if (!unit.Player.IsHostilePlayer(possibleEnemy.Player)) return;
        if (unit.UnitTask.CurrentTask != null && unit.UnitTask.CurrentTask.Type == TaskType.Attack) return;
        if (unit.UnitTask.CurrentTask != null && unit.UnitTask.CurrentTask.Type == TaskType.Hide) return;

        unit.Player.DebugLog("[UnitAlertArea.OnAlertAreaEntered] " + unit.Name + " -> " + possibleEnemy.Name);

        switch (AlertStateOnEnemySight) {
            case AlertState.Hide:
                StartHideTask(possibleEnemy);
                SetDeferred("Monitoring", false);
                break;
            case AlertState.Combat:
                StartCombatTask(possibleEnemy);
                SetDeferred("Monitoring", false);
                break;
        }
    }
    void StartHideTask(NavigationUnit possibleEnemy) {
        alertState = AlertState.Hide;
        UnitTaskHide newHideTask = new(possibleEnemy, unit);
        newHideTask.OnTaskCompletedEvent += OnHideEnd;
        unit.UnitTask.PriorityAddTask(newHideTask);
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