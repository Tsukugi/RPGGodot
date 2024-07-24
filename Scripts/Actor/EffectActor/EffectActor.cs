using Godot;

public abstract partial class EffectActor : ActorBase {
    protected ActorBase target;
    protected EffectBaseDTO attributes;
    protected bool isInitialized = false;
    public delegate void CollideEvent(ActorBase collider);
    public event CollideEvent OnCollideEvent;

    public override void _Ready() {
        base._Ready();
        target = this;
    }

    public void UpdateValues(ActorBase target, EffectBaseDTO attributes) {
        this.target = target;
        this.attributes = attributes;
        isInitialized = true;
    }

    protected void NavigateTo(Vector3 direction, float velocity) {
        Vector3 moveVelocity = GlobalPosition.DirectionTo(direction) * velocity;
        MoveAndSlide(moveVelocity);
    }

    protected void InvokeCollideEvent(ActorBase collider) {
        OnCollideEvent?.Invoke(collider);
    }

    protected ActorBase IsCollisionOnDifferentPlayer(object collider) {
        if (collider is not ActorBase unit || unit.Player.IsSamePlayer(Player)) return null;
        GD.Print(unit.Name);
        return unit;
    }

    protected GodotObject GetCharacterBodyCollider() {
        if (GetLastSlideCollision() is not KinematicCollision3D collision3D) return null;
        return collision3D.GetCollider();
    }
};