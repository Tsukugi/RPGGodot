using Godot;

public partial class PlayerBase : Node {
    CameraBase camera;
    InteractionPanel interactionPanel;
    bool IsPerformanceLogActive = false;
    protected PlayerManager manager;

    public CameraBase Camera { get => camera; }
    public InteractionPanel InteractionPanel { get => interactionPanel; }

    public override void _Ready() {
        base._Ready();
        manager = this.TryFindParentNodeOfType<PlayerManager>();
        camera = GetNodeOrNull<CameraBase>(StaticNodePaths.PlayerCamera);
        interactionPanel = GetNodeOrNull<InteractionPanel>(StaticNodePaths.PlayerUIInteractionPanel);
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (IsPerformanceLogActive) LogPerformance();
    }

    protected void LogPerformance() {
        DebugLog("TimeFps: " + Performance.GetMonitor(Performance.Monitor.TimeFps));
        DebugLog("RenderTotalObjectsInFrame: " + Performance.GetMonitor(Performance.Monitor.RenderTotalObjectsInFrame), true);
        DebugLog("NavigationAgentCount: " + Performance.GetMonitor(Performance.Monitor.NavigationAgentCount));
        DebugLog("TimeNavigationProcess: " + Performance.GetMonitor(Performance.Monitor.TimeNavigationProcess), true);
        DebugLog("TimeProcess: " + Performance.GetMonitor(Performance.Monitor.TimeProcess), true);
        DebugLog("RenderVideoMemUsed: " + Performance.GetMonitor(Performance.Monitor.RenderVideoMemUsed), true);
    }

    public void DebugLog(string message, bool ignore = false) {
        if (ignore) return;
        if (this.IsFirstPlayer()) GD.Print(message);
    }

    public RelationshipType GetRelationship(PlayerBase player) {
        return manager.PlayerRelationship.GetRelationship(Name, player.Name);
    }

    public bool IsHostilePlayer(PlayerBase player) {
        return manager.PlayerRelationship.GetRelationship(Name, player.Name) == RelationshipType.Hostile;
    }
}
