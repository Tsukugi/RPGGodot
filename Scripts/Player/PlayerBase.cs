using Godot;

public partial class PlayerBase : Node3D {
    CameraBase camera;
    InteractionPanel interactionPanel;
    bool IsPerformanceLogActive = false;

    public CameraBase Camera { get => camera; }
    public InteractionPanel InteractionPanel { get => interactionPanel; }


    public override void _Ready() {
        camera = GetNodeOrNull<CameraBase>(StaticNodePaths.PlayerCamera);
        interactionPanel = GetNodeOrNull<InteractionPanel>(StaticNodePaths.PlayerUIInteractionPanel);
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!SimpleGameManager.IsFirstPlayer(this)) return;
        if (IsPerformanceLogActive) LogPerformance();
    }

    protected static void LogPerformance() {
        GD.Print("TimeFps: " + Performance.GetMonitor(Performance.Monitor.TimeFps));
        GD.Print("RenderTotalObjectsInFrame: " + Performance.GetMonitor(Performance.Monitor.RenderTotalObjectsInFrame));
        GD.Print("NavigationAgentCount: " + Performance.GetMonitor(Performance.Monitor.NavigationAgentCount));
        GD.Print("TimeNavigationProcess: " + Performance.GetMonitor(Performance.Monitor.TimeNavigationProcess));
        GD.Print("TimeProcess: " + Performance.GetMonitor(Performance.Monitor.TimeProcess));
        GD.Print("RenderVideoMemUsed: " + Performance.GetMonitor(Performance.Monitor.RenderVideoMemUsed));
    }

}
