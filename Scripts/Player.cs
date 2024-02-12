using Godot;

public partial class Player : ActorBase
{
    [Signal]
    public delegate void HitEventHandler();

    [Export]
    public int speed = 400; // How fast the player will move (piXels/sec).

    private readonly InputHandler inputHandler = new();

    private void MoveNode(Vector3 direction, float delta)
    {
        // Apply velocity.
        direction = direction.Normalized() * speed;
        Position += direction * (float)delta;
    }

    private static Vector3 Vector2To3(Vector2 direction)
    {
        return new Vector3(direction.X, 0, direction.Y);
    }

    public override void _Process(double delta)
    {
        if (playerId != 1) return;
        inputHandler.FrameUpdate();

        Vector2 direction = inputHandler.GetAxis();
        switch (inputHandler.getInputState())
        {
            case InputState.Stop:
                {
                    animationHandler.UpdateAnimationPrefix("idle");
                    animationHandler.ApplyAnimation(inputHandler.getInputFaceDirection());
                    break;
                }
            case InputState.Move:
                {
                    animationHandler.UpdateAnimationPrefix("running");
                    animationHandler.ApplyAnimation(inputHandler.getInputFaceDirection());
                    MoveNode(Vector2To3(direction), (float)delta);
                    break;
                }
        }
    }

    public void Start(Vector2 direction)
    {
        Position = Vector2To3(direction);
    }

}
