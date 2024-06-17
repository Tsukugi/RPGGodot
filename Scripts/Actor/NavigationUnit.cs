

using Godot;

public partial class NavigationUnit : Unit {
    private NavigationAgent3D navigationAgent = null;
    private Sprite3D selectedIndicator = null;
    private Node3D navigationTarget = null;
    private new RealTimeStrategyPlayer Player;
    private bool isSelected = false;

    public NavigationAgent3D NavigationAgent { get => navigationAgent; }
    public Node3D NavigationTarget { get => navigationTarget; }

    public Vector3 NavigationTargetPosition;
    public bool IsSelected {
        get => isSelected;
        set {
            isSelected = value;
            selectedIndicator.Visible = isSelected;
        }
    }

    public override void _Ready() {
        base._Ready();
        Player = (RealTimeStrategyPlayer)GetOwner();

        navigationAgent = GetNodeOrNull<NavigationAgent3D>(StaticNodePaths.NavigationAgent);
        // Make sure to not await during _Ready.
        Callable.From(ActorSetup).CallDeferred();

        selectedIndicator = GetNodeOrNull<Sprite3D>(StaticNodePaths.SelectedIndicator);
        navigationTarget = GetNodeOrNull<Node3D>(StaticNodePaths.NavigationTarget);

    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        OnNavigationMovement();
    }


    private void OnNavigationMovement() {
        if (navigationAgent.TargetPosition != NavigationTargetPosition) {
            // * On Start
            navigationAgent.TargetPosition = NavigationTargetPosition;
            navigationAgent.DebugEnabled = true;
            navigationTarget.Visible = true;
            BodyCollision.Disabled = true;
        } else if (navigationAgent.IsNavigationFinished()) {
            // * On Finish and EveryIteration while Idle
            navigationAgent.DebugEnabled = false;
            navigationTarget.Visible = false;
            BodyCollision.Disabled = false;
        } else {
            // * On EveryIteration while moving
            navigationTarget.GlobalPosition = NavigationTargetPosition;
            Vector3 currentAgentPosition = GlobalTransform.Origin;
            Vector3 nextPathPosition = navigationAgent.GetNextPathPosition();
            Vector3 Velocity = currentAgentPosition.DirectionTo(nextPathPosition) * movementSpeed;
            MoveAndSlide(Velocity);
        }

    }
    private async void ActorSetup() {
        // Wait for the first physics frame so the NavigationServer can sync.
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
    }

}
