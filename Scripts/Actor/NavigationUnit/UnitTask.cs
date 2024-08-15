
public partial class UnitTask : TaskHandler {
    NavigationUnit unit;

    public override void _Ready() {
        base._Ready();
        StartTimer();
        unit = this.TryFindParentNodeOfType<NavigationUnit>();
    }

    protected override void OnTaskProcess() {
        base.OnTaskProcess();
        //! Debug
        AttributesExport attributes = unit.GetAttributes();
        string text = unit.Name + " \n "
            + attributes.HitPoints + "/" + attributes.MaxHitPoints + " \n "
            + unit.AlertArea.AlertState + " \n ";
        if (currentTask != null) {
            text += currentTask.Type.ToString();
            if (unit.UnitCombat.IsInCombat) text += " " + unit.UnitCombat.Target.Name;
        }
        unit.OverheadLabel.Text = text + " \n " + tasks.Count.ToString();
        //! EndDebug
    }
}

