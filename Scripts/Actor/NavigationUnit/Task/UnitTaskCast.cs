using Godot;

public partial class UnitTaskCast : TaskBase {
    Vector3 targetPosition;
    Vector2 unitPositionOnTaskCheck;

    Unit target;

    public UnitTaskCast(Vector3 targetPosition, NavigationUnit unit) {
        type = TaskType.Cast;
        this.targetPosition = targetPosition;
        this.unit = unit;
    }
    public UnitTaskCast(Unit target, NavigationUnit unit) {
        type = TaskType.Cast;
        this.target = target;
        this.unit = unit;
    }

    public override void StartTask() {
        base.StartTask();
        unit.Player.DebugLog("[UnitTask.StartTask] Casting to " + targetPosition);
    }
    public override bool CheckIfCompleted() {
        return base.CheckIfCompleted();
    }

    public override void OnTaskProcess() {

    }

    public override void OnTaskCompleted() {
        base.OnTaskCompleted();
    }

}
