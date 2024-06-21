

using Godot;

public partial class NavigationUnit : Unit {
    NavigationAgent3D navigationAgent = null;
    Sprite3D selectedIndicator = null;
    Node3D navigationTarget = null;
    RealTimeStrategyPlayer player;
    bool isSelected = false;
    AIController aiController = null;

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
    public AIController AiController { get => aiController; }
    public new RealTimeStrategyPlayer Player { get => player; }

    public override void _Ready() {
        base._Ready();
        player = (RealTimeStrategyPlayer)GetOwner();

        navigationAgent = GetNodeOrNull<NavigationAgent3D>(StaticNodePaths.NavigationAgent);
        aiController = GetNodeOrNull<AIController>(StaticNodePaths.AIController);
        // Make sure to not await during _Ready.
        Callable.From(ActorSetup).CallDeferred();

        selectedIndicator = GetNodeOrNull<Sprite3D>(StaticNodePaths.SelectedIndicator);
        navigationTarget = GetNodeOrNull<Node3D>(StaticNodePaths.NavigationTarget);

    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        OnNavigationMovement();
    }


    void OnNavigationMovement() {
        if (navigationAgent.TargetPosition != NavigationTargetPosition) {
            // * On Start
            navigationAgent.TargetPosition = NavigationTargetPosition;
            navigationTarget.Visible = true;
        } else if (navigationAgent.IsNavigationFinished()) {
            // * On Finish and EveryIteration while Idle
            navigationTarget.Visible = false;
        } else {
            // * On EveryIteration while moving
            navigationTarget.GlobalPosition = NavigationTargetPosition;
            Vector3 currentAgentPosition = GlobalTransform.Origin;
            Vector3 nextPathPosition = navigationAgent.GetNextPathPosition();
            Vector3 Velocity = currentAgentPosition.DirectionTo(nextPathPosition) * movementSpeed;
            MoveAndSlide(Velocity);
        }

    }
    async void ActorSetup() {
        // Wait for the first physics frame so the NavigationServer can sync.
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
    }

}
