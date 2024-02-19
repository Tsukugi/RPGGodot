using Godot;

public partial class Character : ActorBase {
    [Signal]
    public delegate void HitEventHandler();

    [Export]
    public int speed = 3; // How fast the player will move (piXels/sec).

    public override void _Process(double delta) {
        if (!SimpleGameManager.IsFirstPlayerControlled(Player)) return;
        ManageInput(delta);
    }

    private void ManageInput(double delta) {
        Player.InputHandler.FrameUpdate();

        Vector2 direction = Player.InputHandler.GetRotatedAxis(-Player.Camera.RotationDegrees.Y);
        switch (Player.InputHandler.GetInputState()) {
            case InputState.Stop: AnimationHandler.UpdateAnimationPrefix("idle"); break;
            case InputState.Move: {
                    AnimationHandler.UpdateAnimationPrefix("running");
                    MoveNode(Vector2To3(direction), (float)delta);
                    break;
                }
        }
        AnimationHandler.ApplyAnimation(Player.InputHandler.GetInputFaceDirection());
    }

    private void MoveNode(Vector3 direction, float delta) {
        // Apply velocity.
        direction = direction.Normalized() * speed;
        MoveAndCollide(direction * (float)delta);
    }

    private static Vector3 Vector2To3(Vector2 direction) {
        return new Vector3(direction.X, 0, direction.Y);
    }

    public void Start(Vector2 direction) {
        Position = Vector2To3(direction);
    }


}
