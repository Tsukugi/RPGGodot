using Godot;

public partial class ProjectileUnit : EffectActor {

    [Export]
    float velocityMultiplier = 1f;

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!isInitialized) return;
        if (IsCollisionOnDifferentPlayer(GetCharacterBodyCollider()) is ActorBase collider) {
            InvokeCollideEvent(collider);
            QueueFree();
            return;
        }
        NavigateTo(target.GlobalPosition, (float)(attributes.velocity * velocityMultiplier));
    }
}
