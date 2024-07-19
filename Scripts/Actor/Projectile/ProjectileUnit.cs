using Godot;

public partial class ProjectileUnit : ActorBase {
    // Dependencies
    RealTimeStrategyPlayer player;
    Vector3 targetPosition;
    float velocity = 0;

    public delegate void OnProjectileEvent();
    public event OnProjectileEvent OnCollideEvent;

    public override void _Ready() {
        base._Ready();
        player = (RealTimeStrategyPlayer)GetOwner();
        targetPosition = GlobalPosition;
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (velocity <= 0) return;
        if (GetLastSlideCollision() is not null) {
            OnCollideEvent?.Invoke();
            QueueFree();
            return;
        }
        NavigateTo(targetPosition);
    }

    public void UpdateValues(Vector3 targetPosition, float velocity) {
        this.targetPosition = targetPosition;
        this.velocity = velocity;
    }

    void NavigateTo(Vector3 direction) {
        Vector3 Velocity = GlobalPosition.DirectionTo(direction) * velocity;
        MoveAndSlide(Velocity);
    }
}
