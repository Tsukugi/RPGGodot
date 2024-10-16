using Godot;

static class AxisNodeNames {
    public static readonly string AttackArea = "AttackArea";
    public static readonly string MeleeCollisionArea = "MeleeCollisionArea";
}
static class AxisNodePaths {
    public static readonly string MeleeAttackArea = "RotationAnchor/AttackArea";
    public static readonly string Effects = "RotationAnchor/Effects";
    public static readonly string RotationAnchor = "RotationAnchor";
}

public partial class AxisUnit : Unit {
    readonly AttackHandler attackHandler = new();
    AttackCollisionArea attackArea = null;
    Node3D rotationAnchor = null;
    EffectAnimationHandler effectAnimationHandler = null;
    new AxisPlayer Player;

    protected Node3D RotationAnchor { get => rotationAnchor; }

    protected EffectAnimationHandler EffectAnimationHandler { get => effectAnimationHandler; }
    protected AttackCollisionArea AttackArea { get => attackArea; }

    public override void _Ready() {
        base._Ready();
        Player = (AxisPlayer)GetOwner();
        // Animation Setup
        rotationAnchor = GetNode<Node3D>(AxisNodePaths.RotationAnchor);
        AnimatedSprite3D effectsSprite = GetNode<AnimatedSprite3D>(AxisNodePaths.Effects);
        effectAnimationHandler = new EffectAnimationHandler(effectsSprite);
        attackArea = GetNode<AttackCollisionArea>(AxisNodePaths.MeleeAttackArea);
        attackArea.AreaEntered += OnMeleeAttackAreaEnteredHandler;
        attackArea.AreaExited += OnMeleeAttackAreaExitedHandler;
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!Player.IsFirstPlayer()) return;
        OnManualInput(delta);
    }

    public override void _Input(InputEvent @event) {
        if (!Player.IsFirstPlayer()) return;
        Player.AxisInputHandler.OnInputUpdate();
    }

    void OnManualInput(double delta) {
        Vector2 direction = Player.AxisInputHandler.GetRotatedAxis(-Player.Camera.RotationDegrees.Y);

        switch (Player.AxisInputHandler.MovementInputState) {
            case InputState.Stop: {
                    ActorAnimationHandler.AnimationPrefix = Constants.AnimationPrefixIdle;
                    break;
                }
            case InputState.Move: {
                    ActorAnimationHandler.AnimationPrefix = Constants.AnimationPrefixRunning;
                    MoveAndCollide(Vector2To3(direction), (float)delta);
                    float rotation = VectorUtils.GetRotationFromDirection(direction);
                    RotationAnchor.RotationDegrees = new Vector3(0, rotation - 90, 0);
                    break;
                }
        }
        ActorAnimationHandler.ApplyAnimation(Player.AxisInputHandler.RenderDirection);

        switch (Player.AxisInputHandler.ActionInputState) {
            case UnitActionState.Attack: {
                    if (attackHandler.OnAttackCooldownTimer != null && attackHandler.OnAttackCooldownTimer.Enabled) break;
                    GD.Print("[StartAttack]");
                    CallDeferred("DeferredUpdateAttackAreaMonitoring", true);
                    attackHandler.StartAttack(AttackAnimationEndHandler, OnAttackCooldownEndHandler, Attributes.AttackCastDuration, Attributes.AttackSpeed);
                    EffectAnimationHandler.ApplyAnimation(Player.AxisInputHandler.ActionInputState);
                    break;
                }
        }
    }

    void OnMeleeAttackAreaEnteredHandler(Area3D area) {
        if (area.Name != AxisNodeNames.AttackArea) return;
        if (!Player.IsFirstPlayer()) return;
        Unit attackedUnit = (Unit)area.GetParent().GetParent();
        GD.Print("[OnMeleeAttackAreaEnteredHandler] " + attackedUnit.Name);
        GD.Print("[OnMeleeAttackAreaEnteredHandler] Applying " + attackedUnit.Attributes.BaseDamage + " of base damage to " + attackedUnit.Name);
        attackedUnit.Attributes.ApplyDamage(attackedUnit.Attributes.BaseDamage);
        GD.Print("[OnMeleeAttackAreaEnteredHandler] HP: " + attackedUnit.Attributes.HitPoints + " / " + attackedUnit.Attributes.MaxHitPoints);

        if (attackedUnit.Attributes.CanBeKilled) {
            attackedUnit.Visible = false;
        }
    }

    void OnMeleeAttackAreaExitedHandler(Area3D area) {
        if (area.Name != AxisNodeNames.AttackArea) return;
        if (!Player.IsFirstPlayer()) return;
        Unit attackedUnit = (Unit)area.GetParent().GetParent();
        GD.Print("[OnMeleeAttackAreaExitedHandler] " + attackedUnit.Name);
    }

    /* This Function is supposed to be called with CallDeferred */
    void DeferredUpdateAttackAreaMonitoring(bool value) {
        AttackArea.Monitoring = value;
    }

    static Vector3 Vector2To3(Vector2 direction) {
        return new Vector3(direction.X, 0, direction.Y);
    }


    void AttackAnimationEndHandler(System.Object source, System.Timers.ElapsedEventArgs e) {
        CallDeferred("DeferredUpdateAttackAreaMonitoring", false);
        GD.Print("[AttackAnimationEndHandler]");
    }

    void OnAttackCooldownEndHandler(System.Object source, System.Timers.ElapsedEventArgs e) {
        GD.Print("[OnAttackCooldownEndHandler]");
    }
}
