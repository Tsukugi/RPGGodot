using Godot;
public abstract partial class ActorBase : CharacterBody3D {

    [Export] // Initial animation direction
    public UnitRenderDirection inputFaceDirection = UnitRenderDirection.Down;

    PlayerBase player = null;
    AnimatedSprite3D animatedSprite3D = null;
    MeshInstance3D meshInstance3D = null;
    Node3D staticRotation = null;
    CollisionShape3D bodyCollision = null;

    protected CollisionShape3D BodyCollision { get => bodyCollision; }
    protected AnimatedSprite3D Sprite { get => animatedSprite3D; }
    protected Node3D StaticRotation { get => staticRotation; }
    public PlayerBase Player { get => player; }


    public override void _Ready() {
        player = GetOwner();
        staticRotation = GetNode<Node3D>(StaticNodePaths.StaticRotation);
        animatedSprite3D = GetNodeOrNull<AnimatedSprite3D>(StaticNodePaths.ActorSprite);
        meshInstance3D = GetNodeOrNull<MeshInstance3D>(StaticNodePaths.ActorMeshInstance);
        bodyCollision = GetNode<CollisionShape3D>(StaticNodePaths.BodyCollision);
        // We are applying rotation on 2d actors only
        if (meshInstance3D is null) Callable.From(ApplyActorRotation).CallDeferred();

    }
    public PlayerBase GetOwner() {
        return this.TryFindParentNodeOfType<PlayerBase>();
    }

    async void ApplyActorRotation() {
        if (CameraBase.CameraTransformOffset.Normalized() == Vector3.Up) return;
        StaticRotation.LookAt(GlobalPosition + CameraBase.CameraTransformOffset, Vector3.Up, true);
    }


    protected void MoveAndCollide(Vector3 direction, float delta) {
        // Apply velocity.
        MoveAndCollide(direction * (float)delta);
    }

    protected void MoveAndSlide(Vector3 direction) {
        // Apply velocity.
        Velocity = direction;
        MoveAndSlide();
    }
}
