using Godot;

public partial class ProjectileUnit : EffectUnit {

    [Export]
    float velocityMultiplier = 1f;
    bool hasReachedTargetAlready = false;

    Vector3 targetPosition = Vector3.Zero;
    Vector3 targetDirection = Vector3.Zero;

    public bool IsTelegraphed = false;

    public delegate void OnTargetReached(Vector3 position);
    public event OnTargetReached OnTargetReachedEvent;

    public override void _Ready() {
        base._Ready();
        Area3D area = GetNode<Area3D>(StaticNodePaths.AreaRange);
        void OnBodyEntered(Node3D collider) {
            if (abilityCastContext.CasterUnit.Name == collider.Name) return;
            else if (GetAllowedColliderPlayer(collider) is Unit colliderUnit) {
                InvokeCollideEvent(colliderUnit);
            }
            ExpireEffect();
        };
        area.BodyEntered += OnBodyEntered;
    }


    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!isInitialized || targetPosition == Vector3.Zero) return;
        if (GlobalPosition.DistanceTo(targetPosition) < 0.3f && !hasReachedTargetAlready) {
            hasReachedTargetAlready = true;
            OnTargetReachedEvent?.Invoke(GlobalPosition);
            return;
        }
        if (IsTelegraphed) {
            NavigateToTarget(targetPosition, attributes.velocity * velocityMultiplier, delta);
        } else {
            NavigateToPosition(targetDirection * attributes.velocity * velocityMultiplier * (float)delta);
        }
    }


    public void SetTargetPosition(Vector3 targetPosition) {
        this.targetPosition = targetPosition;
        targetDirection = GlobalPosition.DirectionTo(targetPosition);
    }
}
