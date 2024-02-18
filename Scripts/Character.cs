using System;
using Godot;

public partial class Character : ActorBase {
    [Signal]
    public delegate void HitEventHandler();

    [Export]
    public int speed = 1; // How fast the player will move (piXels/sec).

    public override void _Process(double delta) {
        if (Player.Name != SimpleGameManager.Player) return;

        Player.InputHandler.FrameUpdate();

        Vector2 direction = Player.InputHandler.GetAxis();
        switch (Player.InputHandler.GetInputState()) {
            case InputState.Stop: {
                    AnimationHandler.UpdateAnimationPrefix("idle");
                    AnimationHandler.ApplyAnimation(Player.InputHandler.GetInputFaceDirection());
                    break;
                }
            case InputState.Move: {
                    AnimationHandler.UpdateAnimationPrefix("running");
                    AnimationHandler.ApplyAnimation(Player.InputHandler.GetInputFaceDirection());
                    MoveNode(Vector2To3(direction), (float)delta);
                    break;
                }
        }
    }

    private void MoveNode(Vector3 direction, float delta) {
        // Apply velocity.
        direction = direction.Normalized() * speed;
        Position += direction * (float)delta;
    }

    private static Vector3 Vector2To3(Vector2 direction) {
        return new Vector3(direction.X, 0, direction.Y);
    }

    public void Start(Vector2 direction) {
        Position = Vector2To3(direction);
    }


}
