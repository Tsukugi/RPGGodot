using Godot;

public partial class UnitTaskMove : TaskBase {
    Vector3 targetPosition;
    readonly float waypointDistanceSafeRadius = 3f;

    public UnitTaskMove(Vector3 targetPosition, NavigationUnit unit) {
        type = TaskType.Move;
        this.targetPosition = targetPosition;
        this.unit = unit;
    }

    public override void StartTask() {
        GD.Print("[UnitTask.StartTask] Moving to " + targetPosition);
        unit.NavigationAgent.StartNewNavigation(targetPosition);
        isAlreadyStarted = true;
    }
    public override bool CheckIfCompleted() {
        float distance = VectorUtils.GetDistanceFromVectors(unit.GlobalPosition.ToVector2(), targetPosition.ToVector2());
        GD.Print("[UnitTask.CheckIfCompleted] Move distance is " + distance);
        return distance < waypointDistanceSafeRadius;
    }

}
