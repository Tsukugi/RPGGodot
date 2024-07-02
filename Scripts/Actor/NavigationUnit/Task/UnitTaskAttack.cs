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
        unit.UnitCombat.OnCombatEndEvent += () => {
            isForceFinished = true;
        };
        unit.UnitCombat.OnAttackFailedEvent += () => {
            if (CheckIfCompleted()) return;
            GetIntoRange();
        };
    }
    public override bool CheckIfCompleted() {
        return base.CheckIfCompleted() || target.Attributes.CanBeKilled || !unit.UnitCombat.IsInCombat;
    }

    public override void OnTaskProcess() {
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
        return VectorUtils.GetDistanceFromVectors(unit.GlobalPosition, target.GlobalPosition) < unit.Attributes.AttackRange;
    }

    public override void OnTaskCompleted() {
        base.OnTaskCompleted();
        unit.NavigationAgent.CancelNavigation();
    }
}