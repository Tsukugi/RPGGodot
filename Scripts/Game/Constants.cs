
public static class Constants {
    /** Animation **/
    public const string AnimationPrefixIdle = "idle";
    public const string AnimationPrefixRunning = "running";
}

public static class StaticNodePaths {
    //* Global *//
    public const string Server = "/root/Server";
    public const string Store = "/root/Store";

    /** Unit **/
    public const string NavigationAgent = "Navigation/NavigationAgent";
    public const string NavigationTarget = "Navigation/NavigationTarget";
    public const string Selection = "Selection";
    public const string Combat = "Combat";
    public const string CombatRayCast = "CombatRayCast";
    public const string Attributes = "Attributes";
    public const string SelectedIndicator = "Selection/SelectedIndicator";
    public const string AIController = "AIController";
    public const string TaskController = "TaskController";
    public const string TaskController_DetectionCast = "TaskController/DetectionCast";
    public const string BodyCollision = "BodyCollision";

    // StaticRotation
    public const string StaticRotation = "StaticRotation";
    public const string ActorSprite = "StaticRotation/AnimatedSprite3D";
    public const string ActorMeshInstance = "StaticRotation/MeshInstance3D";
    public const string OverheadLabel = "StaticRotation/OverheadLabel";

    // Ability
    public const string AbilityIndicator = "AbilityIndicator";

    // Areas
    public const string InteractionArea = "Areas/Interaction";
    public const string AlertArea = "Areas/Alert";
    public const string Area_CollisionShape = "CollisionShape";
    public const string AreaRange = "AreaRange";
    public const string AreaRange_Shape = "AreaRange/Shape";

    /** Player **/
    public const string PlayerSelection = "Selection";
    public const string SelectionCast = "SelectionCast";
    public const string PlayerCamera = "Camera3D";
    public const string PlayerAbility = "Ability";
    public const string PlayerRTSCamera = "RTSCamera";
    public const string PlayerRTSSelection = "RTSSelection";
    public const string PlayerRTSNavigation = "RTSNavigation";
    public const string PlayerRTSUI = "RTSUI";
    public const string PlayerUICanvas = "CanvasLayer";
    public const string PlayerUISelectedUnitInfo = "CanvasLayer/Control/SelectedUnitInfo";
    public const string PlayerUIAbilityBtn = "CanvasLayer/Control/AbilityBtn";
    public const string PlayerUIAbilityBtn2 = "CanvasLayer/Control/AbilityBtn2";
    public const string PlayerUIAbilityBtn3 = "CanvasLayer/Control/AbilityBtn3";
    public const string PlayerUIAbilityBtn4 = "CanvasLayer/Control/AbilityBtn4";
    public const string PlayerUISelectionPanel = "CanvasLayer/Control/SelectionPanel";
    public const string PlayerUIInteractionPanel = "CanvasLayer/Control/InteractionPanel";
    public const string InteractionPanel_Label = "Label";
}

public static class ResourcePaths {
    public const string NavigationUnit = "res://Templates/NavigationUnit.tscn";
    public const string Projectile = "res://Templates/Projectile.tscn";
    public const string AreaOfEffect = "res://Templates/AreaOfEffect.tscn";
}

public static class AbilityCastTypes {
    public const string Target = "target";
    public const string Position = "position";
}

public static class EffectPlayerTypes {
    public const string Self = "Self";
    public const string Friendly = "Friendly";
    public const string Enemy = "Enemy";
    public const string All = "All";
}


public enum CollisionMasks {
    Terrain = 1,
    Actor = 2,
    Wall = 3,
    Projectile = 4,
    Corpse = 32,
}

public enum CameraZoomDirection {
    ZoomIn = -1,
    Idle = 0,
    ZoomOut = 1
}