using Godot;

public partial class PlayerBase : Node {
    // Modules
    protected PlayerAbility playerAbility;

    Server server;

    CameraBase camera;
    InteractionPanel interactionPanel;
    CanvasLayer canvasLayer;
    PlayerWSBind playerWSBind;

    bool IsPerformanceLogActive = false;
    protected PlayerInteractionType currentInteractionType = PlayerInteractionType.None;
    protected PlayerManager playerManager;

    public CameraBase Camera { get => camera; }
    public InteractionPanel InteractionPanel { get => interactionPanel; }
    public CanvasLayer CanvasLayer { get => canvasLayer; }
    public PlayerAbility PlayerAbility { get => playerAbility; }
    public PlayerManager PlayerManager { get => playerManager; }
    public Server Server { get => server; }
    public PlayerWSBind PlayerWSBind { get => playerWSBind; }

    public override void _Ready() {
        base._Ready();
        playerWSBind = new(this, GetNode<Server>(StaticNodePaths.Server));
        server = GetNode<Server>(StaticNodePaths.Server);
        playerManager = this.TryFindParentNodeOfType<PlayerManager>();
        camera = GetNodeOrNull<CameraBase>(StaticNodePaths.PlayerCamera);
        interactionPanel = GetNodeOrNull<InteractionPanel>(StaticNodePaths.PlayerUIInteractionPanel);
        canvasLayer = GetNodeOrNull<CanvasLayer>(StaticNodePaths.PlayerUICanvas);
        playerAbility = GetNodeOrNull<PlayerAbility>(StaticNodePaths.PlayerAbility);
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (IsPerformanceLogActive) LogPerformance();
    }

    // * Interaction
    // We check if Player can interact with a type, a Player cannot interact if he's doing something else
    public bool CanInteract(PlayerInteractionType type) {
        return currentInteractionType == type;
    }
    public void StartInteractionType(PlayerInteractionType type) {
        if (!CanInteract(type)) GD.PrintErr("[CanInteract] Cannot start " + type + ". Already interacting with type " + currentInteractionType);
        DebugLog("[Interact] " + currentInteractionType + " -> " + type, true);
        currentInteractionType = type;
    }
    public virtual void StopInteraction() {
        StartInteractionType(PlayerInteractionType.None);
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
        return playerManager.PlayerRelationship.GetRelationship(Name, player.Name);
    }

    public bool IsHostilePlayer(PlayerBase player) {
        return playerManager.PlayerRelationship.GetRelationship(Name, player.Name) == RelationshipType.Hostile;
    }

    public bool IsSamePlayer(PlayerBase player) {
        return Name == player.Name;
    }
}



// * Add here every kind of interaction that would need to 
// * be the only one active, so we dont have overlappings of 
// * different Player interaction behaviours.
public enum PlayerInteractionType {
    None,
    AbilityCast,
    UnitControl,
}