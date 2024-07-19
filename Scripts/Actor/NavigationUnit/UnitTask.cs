
public partial class UnitTask : TaskHandler {
    NavigationUnit unit;

    public override void _Ready() {
        base._Ready();
        Start();
        unit = this.TryFindParentNodeOfType<NavigationUnit>();
    }

    protected override void OnTaskProcess() {
        base.OnTaskProcess();
        //! Debug
        string text = unit.Name + " \n "
            + unit.Attributes.HitPoints + "/" + unit.Attributes.MaxHitPoints + " \n "
            + unit.AlertArea.AlertState + " \n ";
        if (currentTask != null) {
            text += currentTask.Type.ToString();
            if (unit.UnitCombat.IsInCombat) text += " " + unit.UnitCombat.TargetName;
        }
        unit.OverheadLabel.Text = text + " \n " + tasks.Count.ToString();
        //! EndDebug
    }
}

