using Godot;

public partial class AxisUnit : Unit {
    private AttackCollisionArea attackArea = null;
    readonly private AttackHandler attackHandler = new();
    private Node3D rotationAnchor = null;
    private EffectAnimationHandler effectAnimationHandler = null;

    protected Node3D RotationAnchor { get => rotationAnchor; }

    protected EffectAnimationHandler EffectAnimationHandler { get => effectAnimationHandler; }
    protected AttackCollisionArea AttackArea { get => attackArea; }
    public override void _Ready() {
        base._Ready();
        // Animation Setup
        rotationAnchor = GetNodeOrNull<Node3D>(Constants.RotationAnchor);
        AnimatedSprite3D effectsSprite = GetNodeOrNull<AnimatedSprite3D>(StaticNodePaths.Effects);
        effectAnimationHandler = new EffectAnimationHandler(effectsSprite);
        attackArea = GetNodeOrNull<AttackCollisionArea>(StaticNodePaths.MeleeAttackArea);
        attackArea.AreaEntered += OnMeleeAttackAreaEnteredHandler;
        attackArea.AreaExited += OnMeleeAttackAreaExitedHandler;
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!SimpleGameManager.IsFirstPlayerControlled(Player)) return;
        OnManualInput(delta);
    }

    public override void _Input(InputEvent @event) {
        if (!SimpleGameManager.IsFirstPlayerControlled(Player)) return;
        Player.AxisInputHandler.OnInputUpdate();
    }

    private void OnManualInput(double delta) {
        Vector2 direction = Player.AxisInputHandler.GetRotatedAxis(-Player.Camera.RotationDegrees.Y);

        switch (Player.AxisInputHandler.MovementInputState) {
            case InputState.Stop: {
                    ActorAnimationHandler.AnimationPrefix = Constants.AnimationPrefixIdle;
                    break;
                }
            case InputState.Move: {
                    ActorAnimationHandler.AnimationPrefix = Constants.AnimationPrefixRunning;
                    MoveUnit(Vector2To3(direction), (float)delta);
                    float rotation = VectorUtils.GetRotationFromDirection(direction);
                    RotationAnchor.RotationDegrees = new Vector3(0, rotation - 90, 0);
                    break;
                }
        }
        ActorAnimationHandler.ApplyAnimation(Player.AxisInputHandler.InputFaceDirection);

        switch (Player.AxisInputHandler.ActionInputState) {
            case ActionState.Attack: {
                    if (attackHandler.OnAttackCooldownTimer != null && attackHandler.OnAttackCooldownTimer.Enabled) break;
                    GD.Print("[StartAttack]");
                    CallDeferred("DeferredUpdateAttackAreaMonitoring", true);
                    attackHandler.StartAttack(AttackAnimationEndHandler, OnAttackCooldownEndHandler, Attributes.AttackDuration, Attributes.AttackSpeed);
                    EffectAnimationHandler.ApplyAnimation(Player.AxisInputHandler.ActionInputState);
                    break;
                }
        }
    }

    void OnMeleeAttackAreaEnteredHandler(Area3D area) {
        if (area.Name != Constants.AttackArea) return;
        if (!SimpleGameManager.IsFirstPlayerControlled(Player)) return;
        Unit attackedUnit = (Unit)area.GetParent().GetParent();
        GD.Print("[OnMeleeAttackAreaEnteredHandler] " + attackedUnit.Name);
        GD.Print("[OnMeleeAttackAreaEnteredHandler] Applying " + attackedUnit.Attributes.BaseDamage + " of base damage to " + attackedUnit.Name);
        attackedUnit.Attributes.ApplyDamage(attackedUnit.Attributes.BaseDamage);
        GD.Print("[OnMeleeAttackAreaEnteredHandler] HP: " + attackedUnit.Attributes.HitPoints + " / " + attackedUnit.Attributes.MaxHitPoints);

        if (attackedUnit.Attributes.CanBeKilled()) {
            attackedUnit.Visible = false;
        }
    }

    void OnMeleeAttackAreaExitedHandler(Area3D area) {
        if (area.Name != Constants.AttackArea) return;
        if (!SimpleGameManager.IsFirstPlayerControlled(Player)) return;
        Unit attackedUnit = (Unit)area.GetParent().GetParent();
        GD.Print("[OnMeleeAttackAreaExitedHandler] " + attackedUnit.Name);
    }

    /* This Function is supposed to be called with CallDeferred */
    private void DeferredUpdateAttackAreaMonitoring(bool value) {
        AttackArea.Monitoring = value;
    }

    private static Vector3 Vector2To3(Vector2 direction) {
        return new Vector3(direction.X, 0, direction.Y);
    }


    private void AttackAnimationEndHandler(System.Object source, System.Timers.ElapsedEventArgs e) {
        CallDeferred("DeferredUpdateAttackAreaMonitoring", false);
        GD.Print("[AttackAnimationEndHandler]");
    }

    private void OnAttackCooldownEndHandler(System.Object source, System.Timers.ElapsedEventArgs e) {
        GD.Print("[OnAttackCooldownEndHandler]");
    }
}