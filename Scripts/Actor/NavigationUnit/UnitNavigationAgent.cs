using Godot;

public partial class UnitNavigationAgent : NavigationAgent3D {
    NavigationUnit unit;
    Node3D navigationTarget = null;
    public bool IsMoving { get => IsNavigationFinished(); }

    public override void _Ready() {
        base._Ready();
        unit = this.TryFindParentNodeOfType<NavigationUnit>();
        navigationTarget = unit.GetNodeOrNull<Node3D>(StaticNodePaths.NavigationTarget);
    }


    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        OnNavigationMovement();
    }

    void OnNavigationMovement() {
        if (IsNavigationFinished()) {
            // * On Finish and EveryIteration while Idle
            if (navigationTarget.Visible) navigationTarget.Visible = false;
        } else {
            // * On EveryIteration while moving
            navigationTarget.GlobalPosition = TargetPosition;
            Vector3 nextPathPosition = GetNextPathPosition();
            unit.NavigateTo(nextPathPosition);
        }
    }

    public void CancelNavigation() {
        TargetPosition = unit.GlobalPosition;
    }

    public void StartNewNavigation(Vector3 targetPosition) {
        // * On Start
        TargetPosition = targetPosition;
        navigationTarget.Visible = true;
        unit.Player.DebugLog("[StartNewNavigation] " + unit.Name + " navigates to " + targetPosition);
    }
}