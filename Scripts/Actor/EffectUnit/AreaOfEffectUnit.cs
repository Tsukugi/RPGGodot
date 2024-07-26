using Godot;

public partial class AreaOfEffectUnit : EffectUnit {
    Area3D areaRange;
    CollisionShape3D collisionShape;

    public override void _Ready() {
        base._Ready();
        areaRange = GetNodeOrNull<Area3D>(StaticNodePaths.AreaRange);
        collisionShape = GetNodeOrNull<CollisionShape3D>(StaticNodePaths.AreaRange_Shape);
        ((CapsuleShape3D)collisionShape.Shape).Radius = 0.1f;
        areaRange.BodyEntered += (collider) => {
            if (IsCollisionOnDifferentPlayer(collider) is Unit collidedActor) InvokeCollideEvent(collidedActor, true);
        };
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!isInitialized) return;

        if (Scale.X >= attributes.range) {
            QueueFree();
        }
        Scale += Scale.Magnitude((float)(attributes.velocity * delta));
    }
}
