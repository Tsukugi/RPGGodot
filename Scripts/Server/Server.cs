using Godot;
public partial class Server : Node {
    PlayerManager manager = null;
    readonly WSClient wsClient;

    public WSClient WSClient { get => wsClient; }
    public PlayerManager Manager { get => manager; }

    public Server() {
        wsClient = new(this);
    }

    public override void _Ready() {
        wsClient.Initialize();
    }

    public void ProvidePlayerManager(PlayerManager manager) {
        this.manager = manager;
    }
}
