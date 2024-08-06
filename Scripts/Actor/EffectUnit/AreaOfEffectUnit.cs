using System.Collections.Generic;
using Godot;

public partial class AreaOfEffectUnit : EffectUnit {
    Area3D areaRange;
    CollisionShape3D collisionShape;
    readonly List<Unit> collidedActors = new();

    public override void _Ready() {
        base._Ready();
        areaRange = GetNode<Area3D>(StaticNodePaths.AreaRange);
        collisionShape = GetNode<CollisionShape3D>(StaticNodePaths.AreaRange_Shape);
        ((CapsuleShape3D)collisionShape.Shape).Radius = 0.1f;

        void OnCollide(object collider) {
            if (GetAllowedColliderPlayer(collider) is not Unit collidedActor || collidedActors.Contains(collidedActor)) return;
            InvokeCollideEvent(collidedActor);
            collidedActors.Add(collidedActor);
        }
        areaRange.BodyEntered += OnCollide;
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!isInitialized) return;

        if (Scale.X >= attributes.radius) {
            ExpireEffect();
        }
        Scale += Scale.Magnitude((float)(attributes.velocity * delta));
    }

}
