
using Godot;
public partial class ActorBase : CharacterBody3D {

    [Export] // Initial animation direction
    public UnitRenderDirection inputFaceDirection = UnitRenderDirection.Down;

    PlayerBase player = null;
    AnimatedSprite3D animatedSprite3D = null;
    Node3D staticRotation = null;
    CollisionShape3D bodyCollision = null;

    protected PlayerBase Player { get => player; }

    protected CollisionShape3D BodyCollision { get => bodyCollision; }
    protected AnimatedSprite3D Sprite { get => animatedSprite3D; }
    protected Node3D StaticRotation { get => staticRotation; }

    public override void _Ready() {
        player = GetOwner();
        staticRotation = GetNodeOrNull<Node3D>(StaticNodePaths.StaticRotation);
        animatedSprite3D = GetNodeOrNull<AnimatedSprite3D>(StaticNodePaths.ActorSprite);
        bodyCollision = GetNodeOrNull<CollisionShape3D>(StaticNodePaths.BodyCollision);
        // Make sure to not await during _Ready.
        Callable.From(ApplyActorRotation).CallDeferred();

    }
    public PlayerBase GetOwner() {
        GD.Print("[GetOwner] " + Name + " owner is " + GetParent().Name);
        return GetParent<PlayerBase>();
    }
    async void ApplyActorRotation() {
        if (CameraBase.CameraTransformOffset.Normalized() == Vector3.Up) return;
        StaticRotation.LookAt(Position + CameraBase.CameraTransformOffset);
    }

}

public static class ActorUtils {
    public static Unit TryFindUnit(this Node child) {
        Node currentNode = child;
        while (true) {
            currentNode = currentNode.GetParent();
            if (currentNode == null)  // We went to the root node
                throw new System.Exception("[FindUnitNode] Could not find a unit as a direct or indirect parent of this node");
            if (currentNode is Unit unit) return unit;
        }
    }
}