
using Godot;

public partial class PlayerBase : Node3D {
    private CameraBase camera;
    private InteractionPanel interactionPanel;


    // TODO: Abstract me to use either Axis or Navigation
    public AxisInputHandler AxisInputHandler = new();
    public CameraBase Camera { get => camera; }
    public InteractionPanel InteractionPanel { get => interactionPanel; }
    public override void _Ready() {
        camera = GetNodeOrNull<CameraBase>(StaticNodePaths.PlayerCamera);
        interactionPanel = GetNodeOrNull<InteractionPanel>(StaticNodePaths.PlayerUIInteractionPanel);
    }
}
