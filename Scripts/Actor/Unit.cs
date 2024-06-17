using Godot;

public partial class Unit : ActorBase {

    private Label3D overheadLabel;
    private ActorAnimationHandler actorAnimationHandler = null;
    private Area3D interactionArea = null;
    // These are attributes handy enough to be fast modified on the editor
    [Export]
    protected int movementSpeed = 3; // How fast the player will move (pixels/sec).
    [Export]
    protected int maxHitPoints = 100;
    [Export]
    protected int armor = 1;
    [Export]
    protected int baseDamage = 100;
    protected ActorAnimationHandler ActorAnimationHandler { get => actorAnimationHandler; }
    protected Area3D InteractionArea { get => interactionArea; }

    public readonly UnitAttributes Attributes = new();

    public override void _Ready() {
        base._Ready();
        overheadLabel = GetNodeOrNull<Label3D>(StaticNodePaths.OverheadLabel);
        interactionArea = GetNodeOrNull<Area3D>(Constants.InteractionArea);

        actorAnimationHandler = new ActorAnimationHandler(Sprite) {
            AnimationPrefix = "idle"
        };
        actorAnimationHandler.ApplyAnimation(inputFaceDirection);

        InteractionArea.AreaEntered += OnInteractionAreaEnteredHandler;
        InteractionArea.AreaExited += OnInteractionAreaExitedHandler;

        UpdateUnitAttributes();
    }

    public override void _Process(double delta) {
        if (!Visible && Attributes.CanBeKilled()) {
            // Kill the Unit 
            QueueFree();
            return;
        }

        overheadLabel.Text = "HP: " + Attributes.HitPoints + " / " + Attributes.MaxHitPoints;
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

    private void UpdateUnitAttributes() {
        Attributes.MaxHitPoints = maxHitPoints;
        Attributes.HitPoints = maxHitPoints;
        Attributes.Armor = armor;
        Attributes.BaseDamage = baseDamage;
    }

    protected void MoveUnit(Vector3 direction, float delta) {
        // Apply velocity.
        direction = direction.Normalized() * movementSpeed;
        MoveAndCollide(direction * (float)delta);
    }

    protected void MoveAndSlide(Vector3 direction) {
        // Apply velocity.
        direction = direction.Normalized() * movementSpeed;
        Velocity = direction;
        MoveAndSlide();
    }
}
