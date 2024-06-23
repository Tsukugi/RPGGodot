using Godot;

public partial class UnitTaskAttack : TaskBase {
    Unit target;
    public UnitTaskAttack(Unit target, NavigationUnit unit) {
        type = TaskType.Attack;
        this.target = target;
        this.unit = unit;
    }

    public override void StartTask() {
        GD.Print("[UnitTask.Add] Attacking to " + target.Name);
        unit.NavigationAgent.StartNewNavigation(target.GlobalPosition);
        isAlreadyStarted = true;
    }
    public override bool CheckIfCompleted() {
        // TODO add attack?
        return target.Attributes.CanBeKilled();
    }
}