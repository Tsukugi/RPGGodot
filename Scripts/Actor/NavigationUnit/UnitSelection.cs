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

    public void Select(PlayerBase player) {
        if (unit.Player.IsSamePlayer(player)) {
            unit.AlertArea.CalmDown();
            unit.UnitTask.ClearAll();
        }
        isSelected = true;
        selectedIndicator.Visible = true;
    }

    public void Deselect() {
        isSelected = false;
        selectedIndicator.Visible = false;
    }
}