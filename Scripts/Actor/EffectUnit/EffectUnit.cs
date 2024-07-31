using Godot;

public abstract partial class EffectUnit : ActorBase {

    protected Unit target = null;
    protected EffectBaseDTO attributes;
    protected bool isInitialized = false;
    public bool HasCollidedAlready = false;

    public delegate void CollideEvent(Unit collider);
    public event CollideEvent OnCollideEvent;

    public virtual void UpdateValues(Unit target, EffectBaseDTO attributes) {
        this.target = target;
        this.attributes = attributes;
        isInitialized = true;
    }

    protected void NavigateTo(Vector3 direction, float velocity) {
        Vector3 moveVelocity = GlobalPosition.DirectionTo(direction) * velocity;
        MoveAndSlide(moveVelocity);
    }

    protected void InvokeCollideEvent(Unit collider) {
        HasCollidedAlready = true;
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