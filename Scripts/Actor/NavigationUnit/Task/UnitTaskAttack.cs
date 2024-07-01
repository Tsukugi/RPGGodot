public partial class UnitTaskAttack : TaskBase {
    readonly Unit target;

    public UnitTaskAttack(Unit target, NavigationUnit unit) {
        type = TaskType.Attack;
        this.target = target;
        this.unit = unit;
    }

    public override void StartTask() {
        base.StartTask();
        unit.Player.DebugLog("[UnitTask.Add] Attacking to " + target.Name);
        OnTaskProcess();
    }
    public override bool CheckIfCompleted() {
        return base.CheckIfCompleted() || target.Attributes.CanBeKilled || unit.CombatArea.Target == null;
    }

    public override void OnTaskProcess() {
        if (IsInRange()) {
            /// Attack
            unit.Player.DebugLog("[UnitTaskAttack.OnTaskProcess] " + unit.Name + " should attack to " + target.Name + " as it is in range.", true);
            unit.NavigationAgent.CancelNavigation();
            unit.CombatArea.TryStartAttack();
        } else {
            // Get into range
            unit.CombatArea.StopAttack();
            unit.Player.DebugLog("[UnitTaskAttack.OnTaskProcess] " + unit.Name + " should move to " + target.Name + " as it is not in range.", true);
            unit.NavigationAgent.StartNewNavigation(target.GlobalPosition);
        }
    }

    bool IsInRange() {
        return VectorUtils.GetDistanceFromVectors(unit.GlobalPosition, target.GlobalPosition) < unit.Attributes.AttackRange;
    }

    public override void OnTaskCompleted() {
        base.OnTaskCompleted();
        unit.NavigationAgent.CancelNavigation();
    }
}