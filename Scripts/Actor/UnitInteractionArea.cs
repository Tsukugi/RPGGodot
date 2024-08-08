using Godot;

public partial class UnitInteractionArea : Area3D {
    PlayerBase player;

    public override void _Ready() {
        base._Ready();
        player = this.TryFindParentNodeOfType<PlayerBase>();
        BodyEntered += OnInteractionAreaEnteredHandler;
        BodyExited += OnInteractionAreaExitedHandler;
    }

    void OnInteractionAreaEnteredHandler(Node3D body) {
        if (!player.IsFirstPlayer() || body is not Unit unit || unit.Player.IsFirstPlayer()) return;
        if (unit.Player.Name != "Environment") {
            player.InteractionPanel.Message.Text = "Talk to " + body.Name;
        } else {
            player.InteractionPanel.Message.Text = "Interact with " + body.Name;
        }
        player.InteractionPanel.Visible = true;
    }

    void OnInteractionAreaExitedHandler(Node3D body) {
        if (!player.IsFirstPlayer() || body is not Unit unit || unit.Player.IsFirstPlayer()) return;
        player.InteractionPanel.Visible = false;
    }
}