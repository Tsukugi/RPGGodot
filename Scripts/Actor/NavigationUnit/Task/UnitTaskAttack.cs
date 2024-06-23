public partial class UnitTaskAttack : TaskBase {
    readonly Unit target;
    public UnitTaskAttack(Unit target, NavigationUnit unit) {
        type = TaskType.Attack;
        this.target = target;
        this.unit = unit;
        navigationTargetSafeDistanceRadius = (float)unit.Attributes.AttackRange;
    }

    public override void StartTask() {
        unit.Player.DebugLog("[UnitTask.Add] Attacking to " + target.Name);
        unit.NavigationAgent.StartNewNavigation(target.GlobalPosition);
        isAlreadyStarted = true;
    }
    public override bool CheckIfCompleted() {
        return target.Attributes.CanBeKilled;
    }

    public override void OnTaskInterval() {
        if (IsInRange()) {
            /// Attack
            unit.Player.DebugLog("[UnitTask.Add] " + unit.Name + " should attack to " + target.Name + " as it is in range.");
            unit.NavigationAgent.CancelNavigation();
        } else {
            // Get into range
            unit.Player.DebugLog("[UnitTask.Add] " + unit.Name + " should move to " + target.Name + " as it is not in range.");
            unit.NavigationAgent.StartNewNavigation(target.GlobalPosition);
        }
    }

    public override void OnPhysicsProcess() {
        unit.CombatArea.TryStartAttack();
    }

    bool IsInRange() {
        return VectorUtils.GetDistanceFromVectors(unit.GlobalPosition, target.GlobalPosition) < navigationTargetSafeDistanceRadius;
    }
}