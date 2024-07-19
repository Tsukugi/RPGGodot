using Godot;

public partial class UnitTaskCast : TaskBase {
    string abilityName;
    Vector3 targetPosition;
    Vector2 unitPositionOnTaskCheck;

    Unit target;

    public UnitTaskCast(Vector3 targetPosition, NavigationUnit unit, string abilityName) {
        type = TaskType.Cast;
        this.targetPosition = targetPosition;
        this.unit = unit;
        this.abilityName = abilityName;
    }
    public UnitTaskCast(Unit target, NavigationUnit unit, string abilityName) {
        type = TaskType.Cast;
        this.target = target;
        this.unit = unit;
        this.abilityName = abilityName;
    }

    public override void StartTask() {
        base.StartTask();
        unit.Player.DebugLog("[UnitTask.StartTask] Casting to " + targetPosition);
        unit.CastAbility(abilityName);
        isForceFinished = true;
    }
    public override bool CheckIfCompleted() {
        return base.CheckIfCompleted();
    }

    public override void OnTaskProcess() {
        base.OnTaskProcess();
    }

    public override void OnTaskCompleted() {
        base.OnTaskCompleted();
    }

}
