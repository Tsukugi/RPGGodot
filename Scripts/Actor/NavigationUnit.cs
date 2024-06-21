

using System.Timers;
using Godot;
using Godot.Collections;

public partial class NavigationUnit : Unit {

	// Dependencies
	RealTimeStrategyPlayer player;
	AIController aiController = null;
	Area3D combatArea = null;
	public AIController AiController { get => aiController; }
	public new RealTimeStrategyPlayer Player { get => player; }
	public Area3D CombatArea { get => combatArea; }
	// State
	AlertState alertState = AlertState.Safe;

	// Navigation
	NavigationAgent3D navigationAgent = null;
	Node3D navigationTarget = null;
	public Node3D NavigationTarget { get => navigationTarget; }
	public Vector3 NavigationTargetPosition;
	public bool IsMoving { get => navigationAgent.IsNavigationFinished(); }

	// Selection
	Sprite3D selectedIndicator = null;
	bool isSelected = false;
	public bool IsSelected {
		get => isSelected;
		set {
			isSelected = value;
			selectedIndicator.Visible = isSelected;
		}
	}

	public override void _Ready() {
		base._Ready();
		player = (RealTimeStrategyPlayer)GetOwner();

		navigationAgent = GetNodeOrNull<NavigationAgent3D>(StaticNodePaths.NavigationAgent);
		aiController = GetNodeOrNull<AIController>(StaticNodePaths.AIController);
		combatArea = GetNodeOrNull<Area3D>(StaticNodePaths.CombatArea);
		// Make sure to not await during _Ready.
		Callable.From(ActorSetupProcess).CallDeferred();

		selectedIndicator = GetNodeOrNull<Sprite3D>(StaticNodePaths.SelectedIndicator);
		navigationTarget = GetNodeOrNull<Node3D>(StaticNodePaths.NavigationTarget);
		navigationAgent.WaypointReached += UpdateRenderDirection;
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		OnNavigationMovement();
	}

	void UpdateRenderDirection(Dictionary details) {
		Vector3 distance = VectorUtils.GetDistanceVector(GlobalPosition, navigationAgent.GetNextPathPosition());

		ActorAnimationHandler.ApplyAnimation(
			ActorAnimationHandler.GetRenderDirectionFromVector(
				new Vector2(distance.X, distance.Z)));
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

	async void ActorSetupProcess() {
		// Wait for the first physics frame so the NavigationServer can sync.
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
	}

}

enum AlertState {
	Safe,
	Hide,
	Combat,
}
