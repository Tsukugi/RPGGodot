using Godot;

public partial class AreaOfEffectUnit : EffectActor {

    CollisionShape3D collisionShape;
    public override void _Ready() {
        base._Ready();
        collisionShape = GetNodeOrNull<CollisionShape3D>(StaticNodePaths.AreaRange_Shape);
        ((CapsuleShape3D)collisionShape.Shape).Radius = 0.1f;
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (velocity <= 0) return;
        if (IsCollisionOnDifferentPlayer()) {
            InvokeCollideEvent();
            return;
        }
        ((CapsuleShape3D)collisionShape.Shape).Radius += (float)(velocity * delta);
    }
}
