using Godot;

public abstract partial class EffectUnit : ActorBase {

    bool hasCollidedOnce = false;
    protected Unit target = null;
    protected EffectBaseDTO attributes;
    protected bool isInitialized = false;

    protected bool HasCollidedOnce { get => hasCollidedOnce; }

    public delegate void CollideEvent(Unit collider);
    public event CollideEvent OnCollideEvent;

    public override void _Ready() {
        base._Ready();
    }

    public void UpdateValues(Unit target, EffectBaseDTO attributes) {
        this.target = target;
        this.attributes = attributes;
        isInitialized = true;
    }

    protected void NavigateTo(Vector3 direction, float velocity) {
        Vector3 moveVelocity = GlobalPosition.DirectionTo(direction) * velocity;
        MoveAndSlide(moveVelocity);
    }

    protected void InvokeCollideEvent(Unit collider, bool collideOnce = false) {
        if (collideOnce && hasCollidedOnce) return;
        hasCollidedOnce = true;
        OnCollideEvent?.Invoke(collider);
    }

    protected Unit IsCollisionOnDifferentPlayer(object collider) {
        if (collider is not Unit unit || unit.Player.IsSamePlayer(Player)) return null;
        GD.Print(unit.Name);
        return unit;
    }

    protected GodotObject GetCharacterBodyCollider() {
        if (GetLastSlideCollision() is not KinematicCollision3D collision3D) return null;
        return collision3D.GetCollider();
    }
};