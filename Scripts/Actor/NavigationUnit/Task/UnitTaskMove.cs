using Godot;

public partial class UnitTaskMove : TaskBase {
    Vector3 targetPosition;
    Unit target;

    public UnitTaskMove(Vector3 targetPosition, NavigationUnit unit) {
        type = TaskType.Move;
        this.targetPosition = targetPosition;
        this.unit = unit;
    }
    public UnitTaskMove(Unit target, NavigationUnit unit) {
        type = TaskType.Move;
        this.target = target;
        this.unit = unit;
    }

    public override void StartTask() {
        base.StartTask();
        unit.Player.DebugLog("[UnitTask.StartTask] Moving to " + targetPosition);
        unit.NavigationAgent.StartNewNavigation(targetPosition);
    }
    public override bool CheckIfCompleted() {
        float distance = VectorUtils.GetDistanceFromVectors(unit.GlobalPosition.ToVector2(), targetPosition.ToVector2());
        return base.CheckIfCompleted() || distance < navigationTargetSafeDistanceRadius;
    }

    public override void OnTaskProcess() {
        if (target is null) return;
        targetPosition = target.GlobalPosition;
        unit.NavigationAgent.StartNewNavigation(targetPosition);
    }

    public override void OnTaskCompleted() {
        base.OnTaskCompleted();
        unit.NavigationAgent.CancelNavigation();
    }

}
