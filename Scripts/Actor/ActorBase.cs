
using Godot;
public partial class ActorBase : CharacterBody3D {
    [Export] // Initial animation direction
    public InputFaceDirection inputFaceDirection = InputFaceDirection.Down;


    private AnimatedSprite3D animatedSprite3D = null;
    private AnimationHandler animationHandler = null;


    protected PlayerBase Player = null;
    protected AnimationHandler AnimationHandler { get => animationHandler; }

    public override void _Ready() {
        Player = GetOwner();
        
        animatedSprite3D = GetNode<AnimatedSprite3D>("AnimatedSprite3D");

        if (animatedSprite3D == null) GD.PrintErr("[ActorBase._Ready] Could not find an Animated sprite 3D on this actor");

        animationHandler = new AnimationHandler(animatedSprite3D, "idle");

        if (animationHandler == null) GD.PrintErr("[Player._Ready] Could not find an animationHandler");

        animationHandler.ApplyAnimation(inputFaceDirection);
    }
    public PlayerBase GetOwner() {
        GD.Print("[GetOwner] " + Name + " owner is " + GetParent().Name);
        return GetParent<PlayerBase>();
    }
}
