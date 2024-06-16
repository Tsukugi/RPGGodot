

using Godot;

public partial class NavigationCharacter : Character {

    private NavigationAgent3D navigationAgent = null;
    private Node3D navigationTarget;

    protected Vector3 NavigationMovementTarget {
        get { return navigationAgent.TargetPosition; }
        set { navigationAgent.TargetPosition = value; }
    }
    public NavigationAgent3D NavigationAgent { get => navigationAgent; }
    public Node3D NavigationTarget { get => navigationTarget; }
    private new RealTimeStrategyPlayer Player;

    public override void _Ready() {
        base._Ready();
        Player = (RealTimeStrategyPlayer)GetOwner();

        navigationAgent = GetNode<NavigationAgent3D>(Constants.NavigationAgentPath);
        // These values need to be adjusted for the actor's speed
        // and the navigation layout.
        navigationAgent.PathDesiredDistance = 0.5f;
        navigationAgent.TargetDesiredDistance = 0.5f;
        navigationAgent.DebugEnabled = true;
        // Make sure to not await during _Ready.
        Callable.From(ActorSetup).CallDeferred();

        navigationTarget = GetNode<Node3D>(Constants.NavigationTarget);
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!SimpleGameManager.IsFirstPlayerControlled(Player)) return;
        OnNavigationMovement(delta);
    }

    public override void _Input(InputEvent @event) {
        if (!SimpleGameManager.IsFirstPlayerControlled(Player)) return;
        if (@event is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed && eventMouseButton.ButtonIndex == MouseButton.Right) {
            Vector3? targetPosition = Player.NavigationBase.GetNavigationTargetPosition(Player.Camera);
            if (targetPosition == null) return;
            Player.NavigationInputHandler.worldMouseNavigationTargetCoordinates = (Vector3)targetPosition;
        }
    }


    private void OnNavigationMovement(double delta) {
        if (NavigationMovementTarget != Player.NavigationInputHandler.worldMouseNavigationTargetCoordinates) {
            NavigationMovementTarget = Player.NavigationInputHandler.worldMouseNavigationTargetCoordinates;
            NavigationTarget.Visible = true;
        } else if (NavigationAgent.IsNavigationFinished()) {
            NavigationTarget.Visible = false;
        } else {
            NavigationTarget.GlobalPosition = NavigationMovementTarget;
            Vector3 currentAgentPosition = GlobalTransform.Origin;
            Vector3 nextPathPosition = NavigationAgent.GetNextPathPosition();
            Vector3 Velocity = currentAgentPosition.DirectionTo(nextPathPosition) * movementSpeed;
            MoveCharacter(Velocity, (float)delta);
        }

    }
    private async void ActorSetup() {
        // Wait for the first physics frame so the NavigationServer can sync.
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
    }

}
