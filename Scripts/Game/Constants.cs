
public static class Constants {

    /** Actor **/

    // Nodes
    public static readonly string InteractionArea = "InteractionArea";
    public static readonly string AttackArea = "AttackArea";
    public static readonly string MeleeCollisionArea = "MeleeCollisionArea";
    public static readonly string RotationAnchor = "RotationAnchor";

    /** Animation **/
    public static readonly string AnimationPrefixIdle = "idle";
    public static readonly string AnimationPrefixRunning = "running";
}

public static class StaticNodePaths {
    // Unit
    public static readonly string Effects = "RotationAnchor/Effects";
    public static readonly string MeleeAttackArea = "RotationAnchor/AttackArea";
    public static readonly string ActorSprite = "StaticRotation/AnimatedSprite3D";
    public static readonly string SelectedIndicator = "StaticRotation/SelectedIndicator";
    public static readonly string OverheadLabel = "StaticRotation/OverheadLabel";
    public static readonly string StaticRotation = "StaticRotation";
    public static readonly string NavigationAgent = "NavigationAgent3D";
    public static readonly string NavigationTarget = "NavigationTarget";
    public static readonly string AIController = "AIController";
    public static readonly string BodyCollision = "BodyCollision";
    public static readonly string CombatArea = "CombatArea";
    public static readonly string InteractionPanel_Label = "Label";
    
    // Player  
    public static readonly string PlayerCamera = "Camera3D";
    public static readonly string PlayerUISelectionPanel = "CanvasLayer/Control/SelectionPanel";
    public static readonly string PlayerUIInteractionPanel = "CanvasLayer/Control/InteractionPanel";
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