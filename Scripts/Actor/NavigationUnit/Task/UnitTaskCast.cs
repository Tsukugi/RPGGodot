using Godot;

public partial class UnitTaskCast : TaskBase {
    string abilityName;
    Unit target;

    public UnitTaskCast(Unit target, NavigationUnit unit, string abilityName) {
        type = TaskType.Cast;
        this.target = target;
        this.unit = unit;
        this.abilityName = abilityName;
    }

    public override void StartTask() {
        base.StartTask();
        unit.Player.DebugLog("[UnitTask.StartTask] Casting to " + target.GlobalPosition);
        unit.CastAbility(abilityName, target);
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
