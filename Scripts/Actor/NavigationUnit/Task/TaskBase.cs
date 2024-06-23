using Godot;
public enum TaskType {
    Move, AttackMove, Attack, Interact,
}

public abstract class TaskBase {
    protected NavigationUnit unit;
    protected TaskType type;

    /// <summary>
    /// This method tells the initial actions when the task is started
    /// </summary>
    public abstract void StartTask();
    /// <summary>
    /// This method is to be checked every check interval, and tells if the task can be completed
    /// </summary>
    public abstract bool CheckIfCompleted();
}


public partial class UnitTaskMove : TaskBase {
    Vector3 targetPosition;
    float waypointDistanceSafeRadius = 1f;
    public UnitTaskMove(TaskType type, Vector3 targetPosition, NavigationUnit unit) {
        this.type = type;
        this.targetPosition = targetPosition;
        this.unit = unit;
    }

    public override void StartTask() {
        GD.Print("[UnitTask.Add] Moving to " + targetPosition);
        unit.NavigateTo(targetPosition);
    }
    public override bool CheckIfCompleted() {
        return VectorUtils.GetDistanceFromVectors(unit.GlobalPosition, targetPosition) < waypointDistanceSafeRadius;
    }

}
public partial class UnitTaskAttack : TaskBase {
    Unit target;
    public UnitTaskAttack(TaskType type, Unit target, NavigationUnit unit) {
        this.type = type;
        this.target = target;
        this.unit = unit;
    }

    public override void StartTask() {
        GD.Print("[UnitTask.Add] Attacking to " + target.Name);
        unit.NavigateTo(target.GlobalPosition);
    }
    public override bool CheckIfCompleted() {
        // TODO add attack?
        return target.Attributes.CanBeKilled();
    }

}