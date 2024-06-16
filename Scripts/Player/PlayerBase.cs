
using System;
using Godot;

public partial class PlayerBase : Node {
    private CameraBase camera;
    private InteractionPanel interactionPanel;


    // TODO: Abstract me to use either Axis or Navigation
    public AxisInputHandler AxisInputHandler = new();
    public CameraBase Camera { get => camera; }
    public InteractionPanel InteractionPanel { get => interactionPanel; }
    public override void _Ready() {
        camera = GetNode<CameraBase>("Camera3D");
        interactionPanel = GetNode<InteractionPanel>(Constants.PlayerUIInteractionPanelPath);
    }

}
