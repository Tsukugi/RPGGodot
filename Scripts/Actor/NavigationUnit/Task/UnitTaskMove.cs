using Godot;

public partial class UnitTaskMove : TaskBase {
    Vector3 targetPosition;

    public UnitTaskMove(Vector3 targetPosition, NavigationUnit unit) {
        type = TaskType.Move;
        this.targetPosition = targetPosition;
        this.unit = unit;
    }

    public override void StartTask() {
        unit.Player.DebugLog("[UnitTask.StartTask] Moving to " + targetPosition);
        unit.NavigationAgent.StartNewNavigation(targetPosition);
        isAlreadyStarted = true;
    }
    public override bool CheckIfCompleted() {
        float distance = VectorUtils.GetDistanceFromVectors(unit.GlobalPosition.ToVector2(), targetPosition.ToVector2());
        unit.Player.DebugLog("[UnitTask.CheckIfCompleted] Move distance is " + distance);
        return distance < navigationTargetSafeDistanceRadius;
    }

    public override void OnTaskInterval() {
    }

    public override void OnPhysicsProcess() {
    }
}
