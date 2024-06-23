using Godot;

public partial class PlayerBase : Node {
    PlayerManager manager;
    CameraBase camera;
    InteractionPanel interactionPanel;
    bool IsPerformanceLogActive = false;

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
        DebugLog("RenderTotalObjectsInFrame: " + Performance.GetMonitor(Performance.Monitor.RenderTotalObjectsInFrame));
        DebugLog("NavigationAgentCount: " + Performance.GetMonitor(Performance.Monitor.NavigationAgentCount));
        DebugLog("TimeNavigationProcess: " + Performance.GetMonitor(Performance.Monitor.TimeNavigationProcess));
        DebugLog("TimeProcess: " + Performance.GetMonitor(Performance.Monitor.TimeProcess));
        DebugLog("RenderVideoMemUsed: " + Performance.GetMonitor(Performance.Monitor.RenderVideoMemUsed));
    }

    public void DebugLog(string message) {
        if (this.IsFirstPlayer()) GD.Print(message);
    }
}
