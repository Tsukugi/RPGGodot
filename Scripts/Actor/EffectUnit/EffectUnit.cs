using System;
using Godot;

public abstract partial class EffectUnit : CharacterBody3D {

    Timer expireTimer = new() {
        OneShot = true,
        WaitTime = 10,
    };

    protected UnitPlayerBind unitPlayerBind;
    protected EffectBaseDTO attributes;
    protected bool isInitialized = false;
    protected AbilityCastContext abilityCastContext;

    public PlayerBase Player { get => unitPlayerBind.Player; }
    public bool HasCollidedAlready = false;
    public delegate void ExpireEvent();
    public event ExpireEvent OnExpireEvent;
    public delegate void CollideEvent(Unit collider);
    public event CollideEvent OnCollideEvent;

    public override void _Ready() {
        base._Ready();
        AddChild(expireTimer);
        StartExpireTimer();
        unitPlayerBind = new(this);
    }

    public virtual void InitializeEffectUnit(EffectBaseDTO attributes, AbilityCastContext abilityCastContext) {
        this.attributes = attributes;
        this.abilityCastContext = abilityCastContext;
        isInitialized = true;
    }

    protected void NavigateToTarget(Vector3 targetPosition, double velocity, double delta) {
        Vector3 moveVelocity = GlobalPosition.DirectionTo(targetPosition) * (float)velocity * (float)delta;
        MoveAndCollide(moveVelocity);
    }
    protected void NavigateToPosition(Vector3 targetPosition) {
        MoveAndCollide(targetPosition);
    }

    protected void InvokeCollideEvent(Unit collider) {
        HasCollidedAlready = true;
        OnCollideEvent?.Invoke(collider);
        Player.DebugLog("[OnCollideEvent] " + collider.Name);
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
        return unit;
    }

    protected GodotObject GetCharacterBodyCollider() {
        if (GetLastSlideCollision() is not KinematicCollision3D collision3D) return null;
        return collision3D.GetCollider();
    }

    protected void StartExpireTimer(Action onTimeout = null) {
        if (onTimeout is not null) {
            expireTimer.Timeout += onTimeout;
        } else {

            expireTimer.Timeout += ExpireEffect;
        }
        expireTimer.Start();
    }
    protected void ExpireEffect() {
        Player.DebugLog("[ExpireEffect] " + Name);
        OnExpireEvent?.Invoke();
        QueueFree();
    }
};