
using Godot;
public partial class UnitCombat : UnitCombatBase {
    protected new NavigationUnit unit = null;

    public override void _Ready() {
        base._Ready();
        unit = this.TryFindParentNodeOfType<NavigationUnit>();
    }

    public void StartCombatTask(Unit target) {
        StartCombat(target);
        UnitTaskAttack newAttackTask = new(target, unit);
        void onTaskCompleted(TaskBase task) { EndCombat(); }
        newAttackTask.OnTaskCompletedEvent -= onTaskCompleted;
        newAttackTask.OnTaskCompletedEvent += onTaskCompleted;
        unit.UnitTask.PriorityAddTask(newAttackTask);
    }
}