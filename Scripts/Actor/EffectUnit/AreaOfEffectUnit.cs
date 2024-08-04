using Godot;

public partial class AreaOfEffectUnit : EffectUnit {
    Area3D areaRange;
    CollisionShape3D collisionShape;

    public override void _Ready() {
        base._Ready();
        areaRange = GetNode<Area3D>(StaticNodePaths.AreaRange);
        collisionShape = GetNode<CollisionShape3D>(StaticNodePaths.AreaRange_Shape);
        ((CapsuleShape3D)collisionShape.Shape).Radius = 0.1f;
        areaRange.BodyEntered += (collider) => {
            if (GetAllowedColliderPlayer(collider) is Unit collidedActor && !HasCollidedAlready) InvokeCollideEvent(collidedActor);
        };
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!isInitialized) return;

        if (Scale.X >= attributes.radius) {
            QueueFree();
        }
        Scale += Scale.Magnitude((float)(attributes.velocity * delta));
    }
}
