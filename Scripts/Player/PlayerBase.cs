
using System;
using Godot;

public partial class PlayerBase : Node {

    private InputHandler inputHandler = new();
    private EventHandler<Vector3> onRotationChange;

    public InputHandler InputHandler { get => inputHandler; }

    public override void _Ready() {
        onRotationChange = (sender, eventArgs) => {
            GD.Print(eventArgs);
        };
    }
}
