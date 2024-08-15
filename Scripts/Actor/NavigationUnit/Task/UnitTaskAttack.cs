public partial class UnitTaskAttack : TaskBase {
    readonly Unit target;

    public UnitTaskAttack(Unit target, NavigationUnit unit) {
        type = TaskType.Attack;
        this.target = target;
        this.unit = unit;
    }

    public override void StartTask() {
        base.StartTask();
        unit.Player.DebugLog("[StartTask] Attacking to " + target.Name);
        OnTaskProcess();

        void onCombatEnd() {
            isForceFinished = true;
        }
        unit.UnitCombat.OnCombatEndEvent -= onCombatEnd;
        unit.UnitCombat.OnCombatEndEvent += onCombatEnd;

        void onAttackFailed() {
            if (CheckIfCompleted()) return;
            GetIntoRange();
        };
        unit.UnitCombat.OnAttackFailedEvent -= onAttackFailed;
        unit.UnitCombat.OnAttackFailedEvent += onAttackFailed;
    }
    public override bool CheckIfCompleted() {
        return base.CheckIfCompleted() || target.UnitAttributes.CanBeKilled || !unit.UnitCombat.IsInCombat;
    }

    public override void OnTaskProcess() {
        base.OnTaskProcess();
        if (IsInRange()) ExecuteAttack();
        else GetIntoRange();
    }

    void ExecuteAttack() {
        unit.Player.DebugLog("[UnitTaskAttack.ExecuteAttack] " + unit.Name + " should attack to " + target.Name + " as it is in range.");
        unit.NavigationAgent.CancelNavigation();
        unit.UnitCombat.TryStartAttack();
    }

    void GetIntoRange() {
        unit.UnitCombat.StopAttack();
        unit.Player.DebugLog("[UnitTaskAttack.GetIntoRange] " + unit.Name + " should move to " + target.Name + " as it is not in range.");
        unit.NavigationAgent.StartNewNavigation(target.GlobalPosition);
    }

    bool IsInRange() {
        AttributesExport attributes = unit.GetAttributes();
        return unit.GlobalPosition.DistanceTo(target.GlobalPosition) < attributes.AttackRange;
    }

    public override void OnTaskCompleted() {
        base.OnTaskCompleted();
        unit.NavigationAgent.CancelNavigation();
    }
}