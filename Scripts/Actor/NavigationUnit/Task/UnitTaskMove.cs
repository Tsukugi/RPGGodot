using Godot;

public partial class UnitTaskMove : TaskBase {
    Vector3 targetPosition;
    Vector3 avoidancePosition = Vector3.Zero;
    Vector2 unitPositionOnTaskCheck;

    float minDistanceToStartAvoidance = 1f;
    bool isAvoiding = false;

    float distanceToTarget = 1000;
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
        return base.CheckIfCompleted() || distanceToTarget < unit.NavigationAgent.TargetDesiredDistance;
    }

    void ExecuteManualAvoidance() {
        Vector3 newAvoidancePosition = targetPosition.Rotated(Vector3.Up, 90);
        if (newAvoidancePosition == avoidancePosition) return;
        avoidancePosition = newAvoidancePosition;
        unit.NavigationAgent.StartNewNavigation(avoidancePosition);
    }

    void ResumeOriginalNavigation() {
        if (avoidancePosition == Vector3.Zero) return;
        avoidancePosition = Vector3.Zero;
        unit.NavigationAgent.StartNewNavigation(targetPosition);
    }

    public override void OnTaskProcess() {
        float unitDistanceSinceLastCheck = VectorUtils.GetDistanceFromVectors(unit.GlobalPosition.ToVector2(), unitPositionOnTaskCheck);
        if (unitDistanceSinceLastCheck < minDistanceToStartAvoidance) {
            unit.NavigationAgent.LooseTargetDesiredDistance(0.2f);
            ExecuteManualAvoidance();
        } else {
            ResumeOriginalNavigation();
        }
        distanceToTarget = VectorUtils.GetDistanceFromVectors(unit.GlobalPosition.ToVector2(), targetPosition.ToVector2());
        if (target is not null) {
            targetPosition = target.GlobalPosition;
            unit.NavigationAgent.StartNewNavigation(targetPosition);
        }
        unitPositionOnTaskCheck = unit.GlobalPosition.ToVector2();
    }

    public override void OnTaskCompleted() {
        base.OnTaskCompleted();
        unit.NavigationAgent.ResetTargetDesiredDistance();
        unit.NavigationAgent.CancelNavigation();
    }

}
