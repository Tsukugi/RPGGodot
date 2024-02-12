
using Godot;
public partial class ActorBase : CharacterBody3D
{
    [Export] // Which player this actor belongs to
    public int playerId = 0;
    [Export] // Initial animation direction
    public InputFaceDirection inputFaceDirection = InputFaceDirection.Down;

    private AnimatedSprite3D animatedSprite3D;
    protected AnimationHandler animationHandler;
    public override void _Ready()
    {
        animatedSprite3D = GetNode<AnimatedSprite3D>("AnimatedSprite3D");
        animationHandler = new(animatedSprite3D, "idle");
        animationHandler.ApplyAnimation(inputFaceDirection);
    }

}
