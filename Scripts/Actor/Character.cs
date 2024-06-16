using Godot;

public partial class Character : ActorBase {

    // These are attributes handy enough to be fast modified on the editor
    [Export]
    protected int movementSpeed = 3; // How fast the player will move (pixels/sec).
    [Export]
    protected int maxHitPoints = 100;
    [Export]
    protected int armor = 1;
    [Export]
    protected int baseDamage = 100;

    public readonly CharacterAttributes Attributes = new();
    Label3D label;
    public override void _Ready() {
        base._Ready();
        // ! Debug elements
        label = GetNode<Label3D>("StaticRotation/OverheadLabel");

        InteractionArea.AreaEntered += OnInteractionAreaEnteredHandler;
        InteractionArea.AreaExited += OnInteractionAreaExitedHandler;

        UpdateCharacterAttributes();
    }

    public override void _Process(double delta) {
        if (!Visible && Attributes.CanBeKilled()) {
            // Kill the Character 
            QueueFree();
            return;
        }

        label.Text = "HP: " + Attributes.HitPoints + " / " + Attributes.MaxHitPoints;
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

    protected void MoveCharacter(Vector3 direction, float delta) {
        // Apply velocity.
        direction = direction.Normalized() * movementSpeed;
        MoveAndCollide(direction * (float)delta);
    }


}
