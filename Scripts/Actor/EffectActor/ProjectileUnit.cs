using Godot;

public partial class ProjectileUnit : EffectActor {

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (velocity <= 0) return;
        if (IsCollisionOnDifferentPlayer()) {
            InvokeCollideEvent();
            QueueFree();
            return;
        }
    }

}
