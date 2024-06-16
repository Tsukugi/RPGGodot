using Godot;

public partial class AxisMovementCharacter : Character {
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
        rotationAnchor = GetNode<Node3D>(Constants.RotationAnchor);

        AnimatedSprite3D effectsSprite = GetNode<AnimatedSprite3D>(Constants.EffectsPath);

        if (rotationAnchor == null) GD.PrintErr("[ActorBase._Ready] Could not find an Rotation Anchor for this Actor");
        if (effectsSprite == null) GD.PrintErr("[ActorBase._Ready] Could not find an Animated sprite 3D for Effects on this Actor");

        effectAnimationHandler = new EffectAnimationHandler(effectsSprite);
        attackArea = GetNode<AttackCollisionArea>(Constants.MeleeAttackAreaPath);
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
                    MoveCharacter(Vector2To3(direction), (float)delta);
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
        Character attackedCharacter = (Character)area.GetParent().GetParent();
        GD.Print("[OnMeleeAttackAreaEnteredHandler] " + attackedCharacter.Name);
        GD.Print("[OnMeleeAttackAreaEnteredHandler] Applying " + attackedCharacter.Attributes.BaseDamage + " of base damage to " + attackedCharacter.Name);
        attackedCharacter.Attributes.ApplyDamage(attackedCharacter.Attributes.BaseDamage);
        GD.Print("[OnMeleeAttackAreaEnteredHandler] HP: " + attackedCharacter.Attributes.HitPoints + " / " + attackedCharacter.Attributes.MaxHitPoints);

        if (attackedCharacter.Attributes.CanBeKilled()) {
            attackedCharacter.Visible = false;
        }
    }

    void OnMeleeAttackAreaExitedHandler(Area3D area) {
        if (area.Name != Constants.AttackArea) return;
        if (!SimpleGameManager.IsFirstPlayerControlled(Player)) return;
        Character attackedCharacter = (Character)area.GetParent().GetParent();
        GD.Print("[OnMeleeAttackAreaExitedHandler] " + attackedCharacter.Name);
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