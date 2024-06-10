using Godot;

public partial class Character : ActorBase {
    [Export]
    int movementSpeed = 3; // How fast the player will move (pixels/sec).
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
        // ! Debug elements
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
        if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed && eventMouseButton.ButtonIndex == MouseButton.Right) {

            Vector2 mousePosition = Player.Camera.GetViewport().GetMousePosition();
            Vector3 origin = Player.Camera.ProjectRayOrigin(mousePosition);
            Vector3 direction = Player.Camera.ProjectRayNormal(mousePosition);
            var plane = new Plane(0, -1, 0, 0);
            var position = plane.IntersectsRay(origin, direction);
            if (position == null) return;
            GD.Print((Vector3)position);
            Player.InputHandler.worldMouseNavigationTargetCoordinates = (Vector3)position;
        }
    }

    public override void _Process(double delta) {
        if (!Visible && Attributes.CanBeKilled()) {
            // Kill the Character 
            QueueFree();
            return;
        }

        label.Text = "HP: " + Attributes.HitPoints + " / " + Attributes.MaxHitPoints;

        if (!SimpleGameManager.IsFirstPlayerControlled(Player)) return;
        OnFrameUpdate(delta);
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!SimpleGameManager.IsFirstPlayerControlled(Player)) return;
        OnPhysicsUpdate(delta);
    }
    private void OnPhysicsUpdate(double delta) {
        if (NavigationMovementTarget != Player.InputHandler.WorldMouseNavigationTargetCoordinates) {
            NavigationMovementTarget = Player.InputHandler.WorldMouseNavigationTargetCoordinates;
            NavigationTarget.Visible = true;
        } else if (NavigationAgent.IsNavigationFinished()) {
            NavigationTarget.Visible = false;
        } else {
            NavigationTarget.GlobalPosition = NavigationMovementTarget;
            Vector3 currentAgentPosition = GlobalTransform.Origin;
            Vector3 nextPathPosition = NavigationAgent.GetNextPathPosition();
            Vector3 Velocity = currentAgentPosition.DirectionTo(nextPathPosition) * movementSpeed;
            MoveNode(Velocity, (float)delta);
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
                    GD.Print("[StartAttack]");
                    CallDeferred("DeferredUpdateAttackAreaMonitoring", true);
                    attackHandler.StartAttack(AttackAnimationEndHandler, OnAttackCooldownEndHandler, Attributes.AttackDuration, Attributes.AttackSpeed);
                    EffectAnimationHandler.ApplyAnimation(Player.InputHandler.ActionInputState);
                    break;
                }
        }
    }

    /* This Function is supposed to be called with CallDeferred */
    private void DeferredUpdateAttackAreaMonitoring(bool value) {
        AttackArea.Monitoring = value;
    }

    private void AttackAnimationEndHandler(System.Object source, System.Timers.ElapsedEventArgs e) {
        CallDeferred("DeferredUpdateAttackAreaMonitoring", false);
        GD.Print("[AttackAnimationEndHandler]");
    }

    private void OnAttackCooldownEndHandler(System.Object source, System.Timers.ElapsedEventArgs e) {
        GD.Print("[OnAttackCooldownEndHandler]");
    }

    private void MoveNode(Vector3 direction, float delta) {
        // Apply velocity.
        direction = direction.Normalized() * movementSpeed;
        MoveAndCollide(direction * (float)delta);
    }

    private static Vector3 Vector2To3(Vector2 direction) {
        return new Vector3(direction.X, 0, direction.Y);
    }

    public void Start(Vector2 direction) {
        Position = Vector2To3(direction);
    }

}
