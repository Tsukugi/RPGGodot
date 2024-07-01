
using Godot;
public partial class ActorBase : CharacterBody3D {

    [Export] // Initial animation direction
    public UnitRenderDirection inputFaceDirection = UnitRenderDirection.Down;

    PlayerBase player = null;
    AnimatedSprite3D animatedSprite3D = null;
    Node3D staticRotation = null;
    CollisionShape3D bodyCollision = null;

    protected PlayerBase Player { get => player; }

    protected CollisionShape3D BodyCollision { get => bodyCollision; }
    protected AnimatedSprite3D Sprite { get => animatedSprite3D; }
    protected Node3D StaticRotation { get => staticRotation; }

    public override void _Ready() {
        player = GetOwner();
        staticRotation = GetNodeOrNull<Node3D>(StaticNodePaths.StaticRotation);
        animatedSprite3D = GetNodeOrNull<AnimatedSprite3D>(StaticNodePaths.ActorSprite);
        bodyCollision = GetNodeOrNull<CollisionShape3D>(StaticNodePaths.BodyCollision);
        Callable.From(ApplyActorRotation).CallDeferred();

    }
    public PlayerBase GetOwner() {
        GD.Print("[GetOwner] " + Name + " owner is " + GetParent().Name);
        return GetParent<PlayerBase>();
    }
    async void ApplyActorRotation() {
        if (CameraBase.CameraTransformOffset.Normalized() == Vector3.Up) return;
        StaticRotation.LookAt(GlobalPosition + CameraBase.CameraTransformOffset, Vector3.Up, true);
    }

}
