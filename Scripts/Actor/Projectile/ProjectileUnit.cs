using Godot;

public partial class ProjectileUnit : ActorBase {
    // Dependencies
    Vector3 targetPosition;
    float velocity = 0;

    public delegate void OnProjectileEvent();
    public event OnProjectileEvent OnCollideEvent;

    public override void _Ready() {
        base._Ready();
        targetPosition = GlobalPosition;
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (velocity <= 0) return;
        if (GetLastSlideCollision() is KinematicCollision3D collision3D
            && collision3D.GetCollider() is ActorBase unit
            && !unit.Player.IsSamePlayer(Player)) {

            OnCollideEvent?.Invoke();
            QueueFree();
            return;
        }
        NavigateTo(targetPosition, (float)(velocity * delta));
    }
    public void UpdateValues(Vector3 targetPosition, float velocity) {
        this.targetPosition = targetPosition;
        this.velocity = velocity;
    }

    void NavigateTo(Vector3 direction, float velocity) {
        Vector3 Velocity = GlobalPosition.DirectionTo(direction) * velocity;
        MoveAndSlide(Velocity);
    }
}
