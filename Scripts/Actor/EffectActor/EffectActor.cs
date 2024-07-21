using Godot;

public partial class EffectActor : ActorBase {
    protected Vector3 targetPosition;
    protected float velocity = 0;

    public delegate void EventHandler();
    public event EventHandler OnCollideEvent;

    public override void _Ready() {
        base._Ready();
        targetPosition = GlobalPosition;
    }

    public void UpdateValues(Vector3 targetPosition, float velocity) {
        this.targetPosition = targetPosition;
        this.velocity = velocity;
    }
    
    protected void NavigateTo(Vector3 direction, float velocity) {
        Vector3 Velocity = GlobalPosition.DirectionTo(direction) * velocity;
        MoveAndSlide(Velocity);
    }

    protected void InvokeCollideEvent() {
        OnCollideEvent?.Invoke();
    }

    protected bool IsCollisionOnDifferentPlayer() {
        return GetLastSlideCollision() is KinematicCollision3D collision3D
              && collision3D.GetCollider() is ActorBase unit
              && !unit.Player.IsSamePlayer(Player);
    }
};