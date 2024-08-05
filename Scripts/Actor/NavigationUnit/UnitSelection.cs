using Godot;

public partial class UnitSelection : Node3D {
    NavigationUnit unit;
    Sprite3D selectedIndicator = null;
    bool isSelected = false;
    public bool IsSelected { get => isSelected; }

    public override void _Ready() {
        base._Ready();
        unit = this.TryFindParentNodeOfType<NavigationUnit>();
        selectedIndicator = unit.GetNode<Sprite3D>(StaticNodePaths.SelectedIndicator);
    }

    public void Select(PlayerBase selectorPlayer) {
        if (unit.Player.IsSamePlayer(selectorPlayer)) {
            unit.AlertArea.SetMonitoringEnabled(false);
            unit.UnitTask.ClearAll();
        }
        isSelected = true;
        selectedIndicator.Visible = true;
    }

    public void Deselect() {
        unit.AlertArea.EnableAlertAreaScan();
        isSelected = false;
        selectedIndicator.Visible = false;
    }
}