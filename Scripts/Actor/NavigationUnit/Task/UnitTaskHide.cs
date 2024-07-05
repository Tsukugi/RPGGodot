using Godot;
using Godot.Collections;

public partial class UnitTaskHide : TaskBase {
    readonly Unit target;
    float safeDistanceToHide;
    Vector3 hidePosition;
    bool isHidePositionFound = false;

    public UnitTaskHide(Unit target, NavigationUnit unit) {
        type = TaskType.Hide;
        this.target = target;
        this.unit = unit;
        safeDistanceToHide = target.Attributes.AttackRange + 1f;
    }

    public override void StartTask() {
        base.StartTask();
        unit.Player.DebugLog("[StartTask] Hiding from " + target.Name);
        UpdateHidePosition();
        if (isHidePositionFound) GetAwayFromRange(hidePosition);

    }

    public override bool CheckIfCompleted() {
        return base.CheckIfCompleted() || (IsAwayFromTarget() && unit.NavigationAgent.DistanceToTarget() <= unit.NavigationAgent.TargetDesiredDistance);
    }

    public override void OnTaskProcess() {
        UpdateHidePosition();
        if (isHidePositionFound) GetAwayFromRange(hidePosition);
    }

    public override void OnTaskCompleted() {
        base.OnTaskCompleted();
        unit.NavigationAgent.CancelNavigation();
    }

    void GetAwayFromRange(Vector3 hidePosition) {
        unit.Player.DebugLog("[UnitTaskHide.GetAwayFromRange] " + unit.Name + " will move to " + hidePosition + " as " + target.Name + "is in Range.", true);
        unit.UnitTask.PriorityAddTask(new UnitTaskMove(hidePosition, unit));
    }


    void UpdateHidePosition() {
        Vector3 possibleHidePosition = FindHidePosition();

        if (isHidePositionFound &&
         target.GlobalPosition.DistanceTo(possibleHidePosition) < target.GlobalPosition.DistanceTo(hidePosition)) return;

        hidePosition = possibleHidePosition;
        isHidePositionFound = true;
    }

    Vector3 FindHidePosition() {
        Vector3? possibleHidePosition = null;
        unit.DetectionCast.CollideWithBodies = true;
        unit.DetectionCast.ForceShapecastUpdate();

        if (unit.DetectionCast.IsColliding()) {
            Array collisions = unit.DetectionCast.CollisionResult;
            foreach (var item in collisions) {
                Node3D wallNode = item.AsGodotDictionary()["collider"].As<Node3D>();
                Vector3 position = item.AsGodotDictionary()["point"].As<Vector3>();
                if (CheckIsUselessCover(wallNode) || possibleHidePosition is not null) continue;
                unit.Player.DebugLog("[FindHidePosition]" + position, true);
                possibleHidePosition = FindCoverPosition(position, 1);

            }
        }

        unit.DetectionCast.CollideWithBodies = false;
        if (possibleHidePosition is Vector3 hidePosition) return hidePosition;
        return unit.GlobalPosition - unit.GlobalPosition.FullDirectionTo(target.GlobalPosition);
    }

    bool CheckIsUselessCover(Node3D coverNode) {
        if (coverNode is GridMap) return true;
        if (coverNode is NavigationUnit unitToHideBehind) {
            if (unitToHideBehind.Player.IsHostilePlayer(unit.Player)) return true;
            if (unitToHideBehind.Name == unit.Name) return true;
            if (unitToHideBehind.GlobalPosition == unit.GlobalPosition) return true;
        }
        //if(coverNode.GlobalPosition.DistanceTo)
        return false;
    }

    Vector3 FindCoverPosition(Vector3 position, float offsetRadius) {
        Vector2 direction = target.GlobalPosition.ToVector2().DirectionTo(position.ToVector2()) * offsetRadius;
        // TODO Find collision in gridmap
        unit.Player.DebugLog("[FindCoverPosition]" + position + " " + direction + " " + offsetRadius, true);
        return position + direction.ToVector3();
    }

    bool IsAwayFromTarget() {
        return unit.GlobalPosition.DistanceTo(target.GlobalPosition) > safeDistanceToHide;
    }
}
