using Godot;

public enum UnitActionState {
	Idle,
	Move,
	Attack,
	Cast,
}

public enum UnitRenderDirection {
	Down,
	Up,
	Left,
	Right
}

public partial class Unit : ActorBase {
	ActorAnimationHandler actorAnimationHandler = null;
	Area3D interactionArea = null;
	UnitAttributes attributes;
	protected Label3D overheadLabel;
	public ActorAnimationHandler ActorAnimationHandler { get => actorAnimationHandler; }
	public Area3D InteractionArea { get => interactionArea; }
	public UnitAttributes Attributes { get => attributes; }
	public Label3D OverheadLabel { get => overheadLabel; }
	public bool IsBodyCollisionEnabled { get => !BodyCollision.Disabled; set => BodyCollision.Disabled = !value; }

	public override void _Ready() {
		base._Ready();
		attributes = GetNodeOrNull<UnitAttributes>(StaticNodePaths.Attributes);
		overheadLabel = GetNodeOrNull<Label3D>(StaticNodePaths.OverheadLabel);
		interactionArea = GetNodeOrNull<Area3D>(StaticNodePaths.InteractionArea);
		interactionArea.BodyEntered += OnInteractionAreaEnteredHandler;
		interactionArea.BodyExited += OnInteractionAreaExitedHandler;

		if (Sprite is not null) {
			actorAnimationHandler = new ActorAnimationHandler(Sprite) {
				AnimationPrefix = "idle"
			};
			actorAnimationHandler.ApplyAnimation(inputFaceDirection);
		}

		attributes.OnKilled += OnKilledHandler;
	}

	void OnKilledHandler(Unit unit) {
		// Kill the Unit 
		unit.QueueFree();
	}

	void OnInteractionAreaEnteredHandler(Node3D body) {
		if (!Player.IsFirstPlayer() || body is not Unit unit || unit.Player.IsFirstPlayer()) return;
		if (unit.Player.Name != "Environment") {
			Player.InteractionPanel.Message.Text = "Talk to " + body.Name;
		} else {
			Player.InteractionPanel.Message.Text = "Interact with " + body.Name;
		}
		Player.InteractionPanel.Visible = true;
	}

	void OnInteractionAreaExitedHandler(Node3D body) {
		if (!Player.IsFirstPlayer() || body is not Unit unit || unit.Player.IsFirstPlayer()) return;
		Player.InteractionPanel.Visible = false;
	}


	protected void MoveUnit(Vector3 direction, float delta) {
		// Apply velocity.
		direction = direction.Normalized() * Attributes.MovementSpeed;
		MoveAndCollide(direction * (float)delta);
	}

	protected void MoveAndSlide(Vector3 direction) {
		// Apply velocity.
		direction = direction.Normalized() * Attributes.MovementSpeed;
		Velocity = direction;
		MoveAndSlide();
	}

}
