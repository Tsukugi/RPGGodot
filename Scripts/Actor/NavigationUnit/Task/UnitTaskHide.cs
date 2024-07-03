using Godot;
using Godot.Collections;

public partial class UnitTaskHide : TaskBase {
    readonly Unit target;
    float safeDistanceToHide;
    Vector3 hidePosition;
    bool isHidePositionFound = false;

    ShapeCast3D newCast = new() {
        Shape = new CapsuleShape3D(),
        CollisionMask = (uint)CollisionMasks.Terrain,
        CollideWithBodies = false,
        CollideWithAreas = false,
    };

    public UnitTaskHide(Unit target, NavigationUnit unit) {
        type = TaskType.Hide;
        this.target = target;
        this.unit = unit;
        safeDistanceToHide = unit.AlertArea.AreaRadius;
        ((CapsuleShape3D)newCast.Shape).Radius = safeDistanceToHide / 2;

        unit.AddChild(newCast);
    }

    public override void StartTask() {
        base.StartTask();
        unit.Player.DebugLog("[StartTask] Hiding from " + target.Name);
        UpdateHidePosition();
        if (isHidePositionFound) GetAwayFromRange(hidePosition);

    }

    public override bool CheckIfCompleted() {
        return base.CheckIfCompleted() || IsAwayFromTarget();
    }

    public override void OnTaskProcess() {
        if (isHidePositionFound) return;
        UpdateHidePosition();
        if (isHidePositionFound) GetAwayFromRange(hidePosition);
    }

    public override void OnTaskCompleted() {
        base.OnTaskCompleted();
        if (!newCast.IsQueuedForDeletion()) newCast.QueueFree();
        unit.NavigationAgent.CancelNavigation();
    }

    void GetAwayFromRange(Vector3 hidePosition) {
        unit.Player.DebugLog("[UnitTaskHide.GetAwayFromRange] " + unit.Name + " will move to " + hidePosition + " as " + target.Name + "is in Range.", true);
        unit.NavigationAgent.StartNewNavigation(hidePosition);
    }


    void UpdateHidePosition() {
        Vector3? possibleHidePosition = FindHidePosition();
        if (possibleHidePosition is not Vector3 newHidePosition) return;
        hidePosition = newHidePosition;
        isHidePositionFound = true;
    }

    Vector3? FindHidePosition() {
        Vector3? hidePosition = null;
        newCast.CollideWithBodies = true;
        newCast.ForceShapecastUpdate();

        if (newCast.IsColliding()) {
            Array collisions = newCast.CollisionResult;
            unit.Player.DebugLog("[FindHidePosition]" + collisions[0], true);
            Node3D wallNode = collisions[0].AsGodotDictionary()["collider"].As<Node3D>();
            hidePosition = FindCoverPosition(wallNode);
        }
        newCast.CollideWithBodies = false;
        return hidePosition;
    }

    Vector3 FindCoverPosition(Node3D wallNode) {
        float offset = wallNode.Scale.X;
        Vector2 direction = VectorUtils.GetDistanceVector(unit.GlobalPosition.ToVector2(), wallNode.GlobalPosition.ToVector2()).Normalized() * offset;
        // TODO Find collision in gridmap
        unit.Player.DebugLog("[FindCoverPosition]" + wallNode + " " + wallNode.GlobalPosition + " " + direction + " " + offset, true);
        return wallNode.GlobalPosition + direction.ToVector3();
    }

    bool IsAwayFromTarget() {
        return VectorUtils.GetDistanceFromVectors(unit.GlobalPosition, target.GlobalPosition) > safeDistanceToHide;
    }
}
