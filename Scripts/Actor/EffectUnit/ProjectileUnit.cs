using Godot;

public partial class ProjectileUnit : EffectUnit {

    [Export]
    float velocityMultiplier = 1f;
    bool hasReachedTargetAlready = false;

    Vector3 targetDirection;

    public delegate void OnTargetReached(Vector3 position);
    public event OnTargetReached OnTargetReachedEvent;

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!isInitialized) return;
        if (IsCollisionOnDifferentPlayer(GetCharacterBodyCollider()) is Unit collider && !HasCollidedAlready) {
            InvokeCollideEvent(collider);
            QueueFree();
            return;
        }
        if (GlobalPosition.DistanceTo(targetDirection) < 0.3f && !hasReachedTargetAlready) {
            hasReachedTargetAlready = true;
            OnTargetReachedEvent?.Invoke(GlobalPosition);
            return;
        }
        NavigateTo(targetDirection, (float)(attributes.velocity * velocityMultiplier));
    }

    public void SetTargetDirection(Vector3 targetDirection) {
        this.targetDirection = targetDirection;
    }
}
