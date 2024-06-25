public partial class UnitTaskAttack : TaskBase {
    readonly Unit target;

    public UnitTaskAttack(Unit target, NavigationUnit unit) {
        type = TaskType.Attack;
        this.target = target;
        this.unit = unit;
        navigationTargetSafeDistanceRadius = (float)unit.Attributes.AttackRange;
    }

    public override void StartTask() {
        base.StartTask();
        unit.Player.DebugLog("[UnitTask.Add] Attacking to " + target.Name);
        unit.NavigationAgent.StartNewNavigation(target.GlobalPosition);
    }
    public override bool CheckIfCompleted() {
        return base.CheckIfCompleted() || target.Attributes.CanBeKilled;
    }

    public override void OnTaskProcess() {
        if (IsInRange()) {
            /// Attack
            unit.Player.DebugLog("[UnitTask.Add] " + unit.Name + " should attack to " + target.Name + " as it is in range.");
            unit.NavigationAgent.CancelNavigation();
            unit.CombatArea.TryStartAttack();
        } else {
            // Get into range
            unit.Player.DebugLog("[UnitTask.Add] " + unit.Name + " should move to " + target.Name + " as it is not in range.");
            unit.NavigationAgent.StartNewNavigation(target.GlobalPosition);
        }
    }

    bool IsInRange() {
        return VectorUtils.GetDistanceFromVectors(unit.GlobalPosition, target.GlobalPosition) < navigationTargetSafeDistanceRadius;
    }

    public override void OnTaskCompleted() {
        base.OnTaskCompleted();
        unit.NavigationAgent.CancelNavigation();
    }
}