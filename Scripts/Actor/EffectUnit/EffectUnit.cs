using Godot;

public abstract partial class EffectUnit : ActorBase {

    protected EffectBaseDTO attributes;
    protected bool isInitialized = false;
    public bool HasCollidedAlready = false;
    protected AbilityCastContext abilityCastContext;
    public delegate void CollideEvent(Unit collider);
    public event CollideEvent OnCollideEvent;

    public virtual void InitializeEffectUnit(EffectBaseDTO attributes, AbilityCastContext context) {
        this.attributes = attributes;
        this.abilityCastContext = context;
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


    protected bool ApplyPlayerAffectedTypeFiltering(Unit unit) {
        return attributes.playerTypeAffected switch {
            EffectPlayerTypes.Self => unit.Player.IsSamePlayer(Player),
            EffectPlayerTypes.Enemy => unit.Player.IsHostilePlayer(Player),
            EffectPlayerTypes.Friendly => !unit.Player.IsHostilePlayer(Player),
            EffectPlayerTypes.All => true,
            _ => true,
        };
    }
    protected Unit GetAllowedColliderPlayer(object collider) {
        if (collider is not Unit unit) return null;
        if (!ApplyPlayerAffectedTypeFiltering(unit)) return null;
        GD.Print(unit.Name);
        return unit;
    }

    protected GodotObject GetCharacterBodyCollider() {
        if (GetLastSlideCollision() is not KinematicCollision3D collision3D) return null;
        return collision3D.GetCollider();
    }
};