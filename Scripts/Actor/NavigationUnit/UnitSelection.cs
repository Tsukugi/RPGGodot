using Godot;

public partial class UnitSelection : Node3D {
    NavigationUnit unit;
    Sprite3D selectedIndicator = null;
    bool isSelected = false;
    public bool IsSelected {
        get => isSelected;
        set {
            isSelected = value;
            selectedIndicator.Visible = isSelected;
            if (isSelected) {
                unit.AlertArea.AlertState = AlertState.Safe;
                unit.UnitTask.ClearAll();
            }
        }
    }

    public override void _Ready() {
        base._Ready();
        unit = this.TryFindParentNodeOfType<NavigationUnit>();
        selectedIndicator = unit.GetNodeOrNull<Sprite3D>(StaticNodePaths.SelectedIndicator);
    }
}