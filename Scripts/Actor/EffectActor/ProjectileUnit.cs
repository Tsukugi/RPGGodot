using Godot;

public partial class ProjectileUnit : EffectActor {

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!isInitialized) return;
        if (IsCollisionOnDifferentPlayer(GetCharacterBodyCollider()) is ActorBase collider) {
            InvokeCollideEvent(collider);
            QueueFree();
            return;
        }
        // GD.Print("[ProjectileUnit] attributes " + attributes);
        // GD.Print("[ProjectileUnit] NavigateTo " + target.GlobalPosition);
        NavigateTo(target.GlobalPosition, (float)(attributes.velocity * delta));
    }
}
