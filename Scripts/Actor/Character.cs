using Godot;

public partial class Character : ActorBase {

    [Export]
    int speed = 3; // How fast the player will move (pixels/sec).
    [Export]
    int maxHitPoints = 100;
    [Export]
    int armor = 1;
    [Export]
    int baseDamage = 100;

    public readonly CharacterAttributes Attributes = new();
    readonly AttackHandler attackHandler = new();

    Label3D label;
    public override void _Ready() {
        base._Ready();
        label = GetNode<Label3D>("StaticRotation/OverheadLabel");

        InteractionArea.AreaEntered += OnInteractionAreaEnteredHandler;
        InteractionArea.AreaExited += OnInteractionAreaExitedHandler;

        AttackArea.AreaEntered += OnMeleeAttackAreaEnteredHandler;
        AttackArea.AreaExited += OnMeleeAttackAreaExitedHandler;

        UpdateCharacterAttributes();
    }

    public override void _Input(InputEvent @event) {
        if (!SimpleGameManager.IsFirstPlayerControlled(Player)) return;
        Player.InputHandler.OnInputUpdate();

    }

    public override void _Process(double delta) {
        if (!Visible && Attributes.CanBeKilled()) {
            // TODO: Kill the Character without errors.
            return;
        }

        label.Text = "HP: " + Attributes.HitPoints + " / " + Attributes.MaxHitPoints;

        if (!SimpleGameManager.IsFirstPlayerControlled(Player)) return;
        OnFrameUpdate(delta);
    }

    void OnMeleeAttackAreaEnteredHandler(Area3D area) {
        if (area.Name != Constants.AttackArea) return;
        if (!SimpleGameManager.IsFirstPlayerControlled(Player)) return;
        Character attackedCharacter = (Character)area.GetParent().GetParent();
        GD.Print("[OnMeleeAttackAreaEnteredHandler] " + attackedCharacter.Name);
        GD.Print("[OnMeleeAttackAreaEnteredHandler] Applying " + attackedCharacter.Attributes.BaseDamage + " of base damage to " + attackedCharacter.Name);
        attackedCharacter.Attributes.ApplyDamage(attackedCharacter.Attributes.BaseDamage);
        GD.Print("[OnMeleeAttackAreaEnteredHandler] HP: " + attackedCharacter.Attributes.HitPoints + " / " + attackedCharacter.Attributes.MaxHitPoints);

        if (attackedCharacter.Attributes.CanBeKilled()) attackedCharacter.Visible = false;
    }

    void OnMeleeAttackAreaExitedHandler(Area3D area) {
        if (area.Name != Constants.AttackArea) return;
        if (!SimpleGameManager.IsFirstPlayerControlled(Player)) return;
        Character attackedCharacter = (Character)area.GetParent().GetParent();
        GD.Print("[OnMeleeAttackAreaExitedHandler] " + attackedCharacter.Name);
    }

    void OnInteractionAreaEnteredHandler(Area3D area) {
        if (area.Name != Constants.InteractionArea) return;
        if (!SimpleGameManager.IsFirstPlayerControlled(Player)) return;
        Player.InteractionPanel.Message.Text = "Talk to " + area.GetParent().Name;
        Player.InteractionPanel.Visible = true;
    }

    void OnInteractionAreaExitedHandler(Area3D area) {
        if (area.Name != Constants.InteractionArea) return;
        if (!SimpleGameManager.IsFirstPlayerControlled(Player)) return;
        Player.InteractionPanel.Visible = false;
    }

    private void UpdateCharacterAttributes() {
        Attributes.MaxHitPoints = maxHitPoints;
        Attributes.HitPoints = maxHitPoints;
        Attributes.Armor = armor;
        Attributes.BaseDamage = baseDamage;
    }

    private void OnFrameUpdate(double delta) {
        Vector2 direction = Player.InputHandler.GetRotatedAxis(-Player.Camera.RotationDegrees.Y);

        switch (Player.InputHandler.MovementInputState) {
            case InputState.Stop: {
                    ActorAnimationHandler.AnimationPrefix = Constants.AnimationPrefixIdle;
                    break;
                }
            case InputState.Move: {
                    ActorAnimationHandler.AnimationPrefix = Constants.AnimationPrefixRunning;
                    MoveNode(Vector2To3(direction), (float)delta);
                    float rotation = VectorUtils.GetRotationFromDirection(direction);
                    RotationAnchor.RotationDegrees = new Vector3(0, rotation - 90, 0);
                    break;
                }
        }
        ActorAnimationHandler.ApplyAnimation(Player.InputHandler.InputFaceDirection);

        switch (Player.InputHandler.ActionInputState) {
            case ActionState.Attack: {
                    if (attackHandler.OnAttackCooldownTimer != null && attackHandler.OnAttackCooldownTimer.Enabled) break;
                    GD.Print("[StartAttack]" + AttackArea.Monitoring);
                    CallDeferred("DeferredUpdateAreaMonitoring", true);
                    attackHandler.StartAttack(AttackAnimationEndHandler, OnAttackCooldownEndHandler, Attributes.AttackDuration, Attributes.AttackSpeed);
                    EffectAnimationHandler.ApplyAnimation(Player.InputHandler.ActionInputState);
                    break;
                }
        }
    }

    /* This Function is supposed to be called with CallDeferred */
    private void DeferredUpdateAreaMonitoring(bool value) {
        AttackArea.Monitoring = value;
    }

    private void AttackAnimationEndHandler(System.Object source, System.Timers.ElapsedEventArgs e) {
        CallDeferred("DeferredUpdateAreaMonitoring", false);
        GD.Print("[AttackAnimationEndHandler]");
    }

    private void OnAttackCooldownEndHandler(System.Object source, System.Timers.ElapsedEventArgs e) {
        GD.Print("[OnAttackCooldownEndHandler]");
    }

    private void MoveNode(Vector3 direction, float delta) {
        // Apply velocity.
        direction = direction.Normalized() * speed;
        MoveAndCollide(direction * (float)delta);
    }

    private static Vector3 Vector2To3(Vector2 direction) {
        return new Vector3(direction.X, 0, direction.Y);
    }

    public void Start(Vector2 direction) {
        Position = Vector2To3(direction);
    }

}
