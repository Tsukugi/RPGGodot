using Godot;

public partial class NavigationHandler : CharacterBody3D {
    private NavigationAgent3D navigationAgent;

    private float movementSpeed = 2.0f;
    private Vector3 movementTargetPosition = new Vector3(-3.0f, 0.0f, 2.0f);

    public Vector3 MovementTarget {
        get { return navigationAgent.TargetPosition; }
        set { navigationAgent.TargetPosition = value; }
    }

    public void onReady(NavigationAgent3D navigationAgent) {

        // These values need to be adjusted for the actor's speed
        // and the navigation layout.
        navigationAgent.PathDesiredDistance = 0.5f;
        navigationAgent.TargetDesiredDistance = 0.5f;

        // Make sure to not await during _Ready.
        Callable.From(ActorSetup).CallDeferred();
    }

    public Vector3 onPhysicsProcess(Vector3 currentAgentPosition) {

        if (navigationAgent.IsNavigationFinished()) {
            return Vector3.Zero;
        }

        Vector3 nextPathPosition = navigationAgent.GetNextPathPosition();

        Vector3 Velocity = currentAgentPosition.DirectionTo(nextPathPosition) * movementSpeed;
        return Velocity;
    }

    private async void ActorSetup() {
        // Wait for the first physics frame so the NavigationServer can sync.
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

        // Now that the navigation map is no longer empty, set the movement target.
        MovementTarget = movementTargetPosition;
    }
}