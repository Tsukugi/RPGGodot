
using System;
using Godot;

public partial class PlayerBase : Node {

    private readonly InputHandler inputHandler = new();
    private CameraBase camera;
    private InteractionPanel interactionPanel;


    public InputHandler InputHandler { get => inputHandler; }
    public CameraBase Camera { get => camera; }
    public InteractionPanel InteractionPanel { get => interactionPanel; }

    public override void _Ready() {
        camera = GetNode<CameraBase>("Camera3D");
        interactionPanel = GetNode<InteractionPanel>("CanvasLayer/InteractionPanel");
    }
}
