
public static class Constants {

    /** Actor **/

    // Nodes
    public static readonly string InteractionArea = "InteractionArea";
    public static readonly string AttackArea = "AttackArea";
    public static readonly string MeleeCollisionArea = "MeleeCollisionArea";
    public static readonly string RotationAnchor = "RotationAnchor";

    // Node Paths
    public static readonly string EffectsPath = "RotationAnchor/Effects";
    public static readonly string MeleeAttackAreaPath = "RotationAnchor/AttackArea";
    public static readonly string ActorSpritePath = "StaticRotation/AnimatedSprite3D";
    public static readonly string NavigationAgentPath = "NavigationAgent3D";
    public static readonly string NavigationTarget = "NavigationTarget";
    public static readonly string PlayerUISelectionPanelPath = "CanvasLayer/Control/SelectionPanel";
    public static readonly string PlayerUIInteractionPanelPath = "CanvasLayer/Control/InteractionPanel";

    /** Animation **/
    public static readonly string AnimationPrefixIdle = "idle";
    public static readonly string AnimationPrefixRunning = "running";
}


public enum CollisionMasks {
    Terrain = 1,
    Actor = 2,
}