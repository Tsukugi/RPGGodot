using Godot;

public partial class UnitRender {
    Unit unit;
    AnimatedSprite3D animatedSprite3D = null;
    MeshInstance3D meshInstance3D = null;
    Node3D staticRotation = null;
    CollisionShape3D bodyCollision = null;
    UnitRenderDirection inputFaceDirection = UnitRenderDirection.Down;
    ActorAnimationHandler actorAnimationHandler = null;

    public CollisionShape3D BodyCollision { get => bodyCollision; }
    public AnimatedSprite3D Sprite { get => animatedSprite3D; }
    public Node3D StaticRotation { get => staticRotation; }
    public ActorAnimationHandler ActorAnimationHandler { get => actorAnimationHandler; }

    public UnitRender(Unit unit) {
        this.unit = unit;
        staticRotation = unit.GetNode<Node3D>(StaticNodePaths.StaticRotation);
        animatedSprite3D = unit.GetNodeOrNull<AnimatedSprite3D>(StaticNodePaths.ActorSprite);
        meshInstance3D = unit.GetNodeOrNull<MeshInstance3D>(StaticNodePaths.ActorMeshInstance);
        bodyCollision = unit.GetNode<CollisionShape3D>(StaticNodePaths.BodyCollision);
        // We are applying rotation on 2d actors only
        if (meshInstance3D is null) Callable.From(ApplyActorRotation).CallDeferred();

        if (Sprite is not null) {
            actorAnimationHandler = new ActorAnimationHandler(Sprite) {
                AnimationPrefix = "idle"
            };
            actorAnimationHandler.ApplyAnimation(inputFaceDirection);
        }
    }

    void ApplyActorRotation() {
        if (CameraBase.CameraTransformOffset.Normalized() == Vector3.Up) return;
        StaticRotation.LookAt(unit.GlobalPosition + CameraBase.CameraTransformOffset, Vector3.Up, true);
    }


}