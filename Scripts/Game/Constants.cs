
public static class Constants {
    /** Animation **/
    public static readonly string AnimationPrefixIdle = "idle";
    public static readonly string AnimationPrefixRunning = "running";
}

public static class StaticNodePaths {
    /** Unit **/
    public static readonly string NavigationAgent = "Navigation/NavigationAgent";
    public static readonly string NavigationTarget = "Navigation/NavigationTarget";
    public static readonly string Selection = "Selection";
    public static readonly string SelectedIndicator = "Selection/SelectedIndicator";
    public static readonly string AIController = "AIController";
    public static readonly string BodyCollision = "BodyCollision";

    // StaticRotation
    public static readonly string StaticRotation = "StaticRotation";
    public static readonly string ActorSprite = "StaticRotation/AnimatedSprite3D";
    public static readonly string OverheadLabel = "StaticRotation/OverheadLabel";

    // Areas
    public static readonly string InteractionArea = "Areas/Interaction";
    public static readonly string CombatArea = "Areas/Combat";
    public static readonly string AlertArea = "Areas/Alert";
    public static readonly string Area_CollisionShape = "CollisionShape";

    /** Player **/
    public static readonly string PlayerCamera = "Camera3D";
    public static readonly string PlayerUISelectionPanel = "CanvasLayer/Control/SelectionPanel";
    public static readonly string PlayerUIInteractionPanel = "CanvasLayer/Control/InteractionPanel";
    public static readonly string InteractionPanel_Label = "Label";
}


public enum CollisionMasks {
    Terrain = 1,
    Actor = 2,
}

public enum CameraZoomDirection {
    ZoomIn = -1,
    Idle = 0,
    ZoomOut = 1
}