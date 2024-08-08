using Godot;

public partial class AURenderer2D : Node3D {

    AxisUnit unit;

    AttackCollisionArea attackArea = null;
    EffectAnimationHandler effectAnimationHandler = null;

    public EffectAnimationHandler EffectAnimationHandler { get => effectAnimationHandler; }
    public AttackCollisionArea AttackArea { get => attackArea; }
    public override void _Ready() {
        base._Ready();
        unit = this.TryFindParentNodeOfType<AxisUnit>();
        // Animation Setup
        AnimatedSprite3D effectsSprite = GetNode<AnimatedSprite3D>(AxisNodePaths.Effects);
        effectAnimationHandler = new EffectAnimationHandler(effectsSprite);
        attackArea = GetNode<AttackCollisionArea>(AxisNodePaths.MeleeAttackArea);
    }


    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!unit.Player.IsFirstPlayer()) return;
        Vector2 direction = unit.Player.AxisInputHandler.GetRotatedAxis(-unit.Player.Camera.RotationDegrees.Y);

        switch (unit.Player.AxisInputHandler.MovementInputState) {
            case InputState.Stop: {
                    unit.UnitRender.ActorAnimationHandler.AnimationPrefix = Constants.AnimationPrefixIdle;
                    break;
                }
            case InputState.Move: {
                    unit.UnitRender.ActorAnimationHandler.AnimationPrefix = Constants.AnimationPrefixRunning;

                    float rotation = VectorUtils.GetRotationFromDirection(direction);
                    RotationDegrees = new Vector3(0, rotation - 90, 0);
                    break;
                }
        }
        unit.UnitRender.ActorAnimationHandler.ApplyAnimation(unit.Player.AxisInputHandler.RenderDirection);

        switch (unit.Player.AxisInputHandler.ActionInputState) {
            case UnitActionState.Attack: {
                    if (unit.AttackHandler.OnAttackCooldownTimer != null && unit.AttackHandler.OnAttackCooldownTimer.Enabled) break;
                    unit.Player.DebugLog("[StartAttack]", true);
                    Callable.From(() => UpdateAttackAreaMonitoring(true)).CallDeferred();
                    unit.AttackHandler.StartAttack(AttackAnimationEndHandler, OnAttackCooldownEndHandler, unit.Attributes.AttackCastDuration, unit.Attributes.AttackSpeed);
                    effectAnimationHandler.ApplyAnimation(unit.Player.AxisInputHandler.ActionInputState);
                    break;
                }
        }
    }
    
    void UpdateAttackAreaMonitoring(bool value) {
        attackArea.Monitoring = value;
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