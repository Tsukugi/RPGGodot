using Godot;

public partial class PlayerBase : Node {
    CameraBase camera;
    InteractionPanel interactionPanel;
    CanvasLayer canvasLayer;
    bool IsPerformanceLogActive = false;
    protected PlayerManager manager;

    public CameraBase Camera { get => camera; }
    public InteractionPanel InteractionPanel { get => interactionPanel; }
    public CanvasLayer CanvasLayer { get => canvasLayer; }

    public override void _Ready() {
        base._Ready();
        manager = this.TryFindParentNodeOfType<PlayerManager>();
        camera = GetNodeOrNull<CameraBase>(StaticNodePaths.PlayerCamera);
        interactionPanel = GetNodeOrNull<InteractionPanel>(StaticNodePaths.PlayerUIInteractionPanel);
        canvasLayer = GetNodeOrNull<CanvasLayer>(StaticNodePaths.PlayerUICanvas);
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (IsPerformanceLogActive) LogPerformance();
    }

    protected void LogPerformance() {
        DebugLog("TimeFps: " + Performance.GetMonitor(Performance.Monitor.TimeFps), true);
        DebugLog("RenderTotalObjectsInFrame: " + Performance.GetMonitor(Performance.Monitor.RenderTotalObjectsInFrame));
        DebugLog("NavigationAgentCount: " + Performance.GetMonitor(Performance.Monitor.NavigationAgentCount));
        DebugLog("TimeNavigationProcess: " + Performance.GetMonitor(Performance.Monitor.TimeNavigationProcess));
        DebugLog("TimeProcess: " + Performance.GetMonitor(Performance.Monitor.TimeProcess));
        DebugLog("RenderVideoMemUsed: " + Performance.GetMonitor(Performance.Monitor.RenderVideoMemUsed));
    }

    public void DebugLog(string message, bool show = false) {
        if (show) GD.Print(Name + " -> " + message);
    }

    public RelationshipType GetRelationship(PlayerBase player) {
        return manager.PlayerRelationship.GetRelationship(Name, player.Name);
    }

    public bool IsHostilePlayer(PlayerBase player) {
        return manager.PlayerRelationship.GetRelationship(Name, player.Name) == RelationshipType.Hostile;
    }

    public bool IsSamePlayer(PlayerBase player) {
        return Name == player.Name;
    }
}
