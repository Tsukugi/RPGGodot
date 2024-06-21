
using Godot;
public partial class ActorBase : CharacterBody3D {

    [Export] // Initial animation direction
    public UnitRenderDirection inputFaceDirection = UnitRenderDirection.Down;

    PlayerBase player = null;
    AnimatedSprite3D animatedSprite3D = null;
    CollisionShape3D bodyCollision = null;

    protected PlayerBase Player { get => player; }

    protected CollisionShape3D BodyCollision { get => bodyCollision; }
    protected AnimatedSprite3D Sprite { get => animatedSprite3D; }

    public override void _Ready() {
        player = GetOwner();
        animatedSprite3D = GetNodeOrNull<AnimatedSprite3D>(StaticNodePaths.ActorSprite);
        bodyCollision = GetNodeOrNull<CollisionShape3D>(StaticNodePaths.BodyCollision);

    }
    public PlayerBase GetOwner() {
        GD.Print("[GetOwner] " + Name + " owner is " + GetParent().Name);
        return GetParent<PlayerBase>();
    }
}
