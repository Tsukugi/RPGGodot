using Godot;

public partial class ProjectileUnit : EffectUnit {

    [Export]
    float velocityMultiplier = 1f;

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!isInitialized) return;
        if (IsCollisionOnDifferentPlayer(GetCharacterBodyCollider()) is Unit collider) {
            InvokeCollideEvent(collider, true);
            QueueFree();
            return;
        }
        NavigateTo(target.GlobalPosition, (float)(attributes.velocity * velocityMultiplier));
    }
}
