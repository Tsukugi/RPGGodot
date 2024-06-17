
using Godot;
public partial class ActorBase : CharacterBody3D {

    [Export] // Initial animation direction
    public InputFaceDirection inputFaceDirection = InputFaceDirection.Down;

    private PlayerBase player = null;

    private AnimatedSprite3D animatedSprite3D = null;
    private ActorAnimationHandler actorAnimationHandler = null;
    private Area3D interactionArea = null;
    private CollisionShape3D bodyCollision = null;

    protected PlayerBase Player { get => player; }
    protected ActorAnimationHandler ActorAnimationHandler { get => actorAnimationHandler; }
    protected Area3D InteractionArea { get => interactionArea; }
    protected CollisionShape3D BodyCollision { get => bodyCollision; }

    public override void _Ready() {
        player = GetOwner();

        interactionArea = GetNode<Area3D>(Constants.InteractionArea);
        bodyCollision = GetNode<CollisionShape3D>(Constants.BodyCollisionPath);
        animatedSprite3D = GetNode<AnimatedSprite3D>(Constants.ActorSpritePath);
        if (animatedSprite3D == null) GD.PrintErr("[ActorBase._Ready] Could not find an Animated sprite 3D on this Actor");

        actorAnimationHandler = new ActorAnimationHandler(animatedSprite3D) {
            AnimationPrefix = "idle"
        };


        actorAnimationHandler.ApplyAnimation(inputFaceDirection);
    }
    public PlayerBase GetOwner() {
        GD.Print("[GetOwner] " + Name + " owner is " + GetParent().Name);
        return GetParent<PlayerBase>();
    }
}
