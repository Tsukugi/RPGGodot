using Godot;

public partial class UnitNavigationAgent : NavigationAgent3D {
    NavigationUnit unit;
    Node3D navigationTarget = null;
    Vector3 navigationTargetPosition;
    public Node3D NavigationTarget { get => navigationTarget; }
    public bool IsMoving { get => this.IsNavigationFinished(); }
    public Vector3 NavigationTargetPosition {
        get => navigationTargetPosition;
        set {
            navigationTargetPosition = value;
            unit.UnitTask.Add(new UnitTaskMove(TaskType.Move, navigationTargetPosition, unit));
        }
    }

    public override void _Ready() {
        base._Ready();
        unit = this.FindNavigationUnit();
        navigationTarget = unit.GetNodeOrNull<Node3D>(StaticNodePaths.NavigationTarget);
    }


    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (unit.AlertArea.AlertState == AlertState.Safe) {
            OnNavigationMovement();
        }
    }

    void OnNavigationMovement() {
        if (TargetPosition != NavigationTargetPosition) {
            // * On Start
            TargetPosition = NavigationTargetPosition;
            navigationTarget.Visible = true;
        } else if (IsNavigationFinished()) {
            // * On Finish and EveryIteration while Idle
            navigationTarget.Visible = false;
        } else {
            // * On EveryIteration while moving
            navigationTarget.GlobalPosition = NavigationTargetPosition;
            Vector3 currentAgentPosition = unit.GlobalTransform.Origin;
            Vector3 nextPathPosition = GetNextPathPosition();
            unit.NavigateTo(nextPathPosition);
        }
    }
}