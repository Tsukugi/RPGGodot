using Godot;

public partial class Character : ActorBase {

    [Export]
    public int speed = 3; // How fast the player will move (pixels/sec).
    [Export]
    public int maxHitPoints = 100;
    [Export]
    public int armor = 1;
    [Export]
    public int baseDamage = 100;

    readonly CharacterAttributes attributes = new();

    public override void _Ready() {
        base._Ready();
        UpdateCharacterAttributes();
    }

    public override void _Input(InputEvent @event) {
        if (!SimpleGameManager.IsFirstPlayerControlled(Player)) return;
        Player.InputHandler.OnInputUpdate();
    }

    public override void _Process(double delta) {
        if (!SimpleGameManager.IsFirstPlayerControlled(Player)) return;
        ManageInput(delta);
    }

    private void UpdateCharacterAttributes() {
        attributes.MaxHitPoints = maxHitPoints;
        attributes.HitPoints = maxHitPoints;
        attributes.Armor = armor;
        attributes.BaseDamage = baseDamage;
    }

    private void ManageInput(double delta) {
        Vector2 direction = Player.InputHandler.GetRotatedAxis(-Player.Camera.RotationDegrees.Y);
        switch (Player.InputHandler.MovementInputState) {

            case InputState.Stop: {
                    ActorAnimationHandler.AnimationPrefix = "idle";
                    break;
                }
            case InputState.Move: {
                    ActorAnimationHandler.AnimationPrefix = "running";
                    MoveNode(Vector2To3(direction), (float)delta);
                    break;
                }
        }

        switch (Player.InputHandler.ActionInputState) {
            case ActionState.Attack: {
                    // Do something
                    break;
                }

        }
        EffectAnimationHandler.ApplyAnimation(Player.InputHandler.ActionInputState);
        ActorAnimationHandler.ApplyAnimation(Player.InputHandler.InputFaceDirection);
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
