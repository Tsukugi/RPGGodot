

using Godot;

public partial class NavigationCharacter : Character {
    private NavigationAgent3D navigationAgent = null;
    private Node3D navigationTarget;
    private new RealTimeStrategyPlayer Player;

    public NavigationAgent3D NavigationAgent { get => navigationAgent; }
    public Node3D NavigationTarget { get => navigationTarget; }
    public Vector3 NavigationTargetPosition;
    public bool isSelected = false;

    public override void _Ready() {
        base._Ready();
        Player = (RealTimeStrategyPlayer)GetOwner();

        navigationAgent = GetNode<NavigationAgent3D>(Constants.NavigationAgentPath);
        // Make sure to not await during _Ready.
        Callable.From(ActorSetup).CallDeferred();

        navigationTarget = GetNode<Node3D>(Constants.NavigationTarget);
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        OnNavigationMovement();
    }

    public override void _Input(InputEvent @event) {
        if (!isSelected) return;

    }


    private void OnNavigationMovement() {
        if (navigationAgent.TargetPosition != NavigationTargetPosition) {
            navigationAgent.TargetPosition = NavigationTargetPosition;
            NavigationTarget.Visible = true;
            NavigationAgent.DebugEnabled = true;
        } else if (NavigationAgent.IsNavigationFinished()) {
            NavigationTarget.Visible = false;
            NavigationAgent.DebugEnabled = false;
        } else {
            NavigationTarget.GlobalPosition = NavigationTargetPosition;
            Vector3 currentAgentPosition = GlobalTransform.Origin;
            Vector3 nextPathPosition = NavigationAgent.GetNextPathPosition();
            Vector3 Velocity = currentAgentPosition.DirectionTo(nextPathPosition) * movementSpeed;
            MoveAndSlide(Velocity);
        }

    }
    private async void ActorSetup() {
        // Wait for the first physics frame so the NavigationServer can sync.
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
    }

}
