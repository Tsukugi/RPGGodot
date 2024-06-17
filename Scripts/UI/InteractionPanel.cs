using Godot;

public partial class InteractionPanel : HBoxContainer {
    private Label message;

    public Label Message { get => message; }

    public override void _Ready() {
        message = GetNode<Label>(StaticNodePaths.InteractionPanel_Label);
    }


}
