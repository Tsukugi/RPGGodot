
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
    public static readonly string Combat = "Combat";
    public static readonly string CombatRayCast = "CombatRayCast";
    public static readonly string Attributes = "Attributes";
    public static readonly string SelectedIndicator = "Selection/SelectedIndicator";
    public static readonly string AIController = "AIController";
    public static readonly string TaskController = "TaskController";
    public static readonly string TaskController_DetectionCast = "TaskController/DetectionCast";
    public static readonly string BodyCollision = "BodyCollision";

    // StaticRotation
    public static readonly string StaticRotation = "StaticRotation";
    public static readonly string ActorSprite = "StaticRotation/AnimatedSprite3D";
    public static readonly string ActorMeshInstance = "StaticRotation/MeshInstance3D";
    public static readonly string OverheadLabel = "StaticRotation/OverheadLabel";

    // Areas
    public static readonly string InteractionArea = "Areas/Interaction";
    public static readonly string AlertArea = "Areas/Alert";
    public static readonly string Area_CollisionShape = "CollisionShape";
    public static readonly string AreaRange = "AreaRange";
    public static readonly string AreaRange_Shape = "AreaRange/Shape";

    /** Player **/
    public static readonly string PlayerInput = "Input";
    public static readonly string PlayerSelection = "Selection";
    public static readonly string PlayerSelectionCast = "Selection/SelectionCast";
    public static readonly string PlayerNavigation = "Navigation";
    public static readonly string PlayerCamera = "Camera3D";

    public static readonly string PlayerUICanvas = "CanvasLayer";

    public static readonly string PlayerUISelectedUnitInfo = "CanvasLayer/Control/SelectedUnitInfo";

    public static readonly string PlayerUIAbilityBtn = "CanvasLayer/Control/AbilityBtn";
    public static readonly string PlayerUIAbilityBtn2 = "CanvasLayer/Control/AbilityBtn2";
    public static readonly string PlayerUIAbilityBtn3 = "CanvasLayer/Control/AbilityBtn3";
    public static readonly string PlayerUIAbilityBtn4 = "CanvasLayer/Control/AbilityBtn4";
    public static readonly string PlayerUISelectionPanel = "CanvasLayer/Control/SelectionPanel";
    public static readonly string PlayerUIInteractionPanel = "CanvasLayer/Control/InteractionPanel";
    public static readonly string InteractionPanel_Label = "Label";
}

public static class ResourcePaths {
    public static readonly string NavigationUnit = "res://Templates/NavigationUnit.tscn";
    public static readonly string Projectile = "res://Templates/Projectile.tscn";
    public static readonly string AreaOfEffect = "res://Templates/AreaOfEffect.tscn";
}


public enum CollisionMasks {
    Terrain = 1,
    Actor = 2,
    Projectile = 4,
}

public enum CameraZoomDirection {
    ZoomIn = -1,
    Idle = 0,
    ZoomOut = 1
}