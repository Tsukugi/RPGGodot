
using Godot;
public partial class ActorBase : CharacterBody3D {
	
	[Export] // Initial animation direction
	public InputFaceDirection inputFaceDirection = InputFaceDirection.Down;

	private AnimatedSprite3D animatedSprite3D = null;
	private ActorAnimationHandler actorAnimationHandler = null;
	private EffectAnimationHandler effectAnimationHandler = null;
	private Area3D interactionArea = null;
	private AttackCollisionArea attackArea = null;
	private Node3D rotationAnchor = null;
	private PlayerBase player = null;

	protected PlayerBase Player { get => player; }
	protected ActorAnimationHandler ActorAnimationHandler { get => actorAnimationHandler; }
	protected EffectAnimationHandler EffectAnimationHandler { get => effectAnimationHandler; }
	protected Area3D InteractionArea { get => interactionArea; }
	protected AttackCollisionArea AttackArea { get => attackArea; }
	protected Node3D RotationAnchor { get => rotationAnchor; }

	public override void _Ready() {
		player = GetOwner();

		interactionArea = GetNode<Area3D>(Constants.InteractionArea);
		attackArea = GetNode<AttackCollisionArea>(Constants.MeleeAttackAreaPath);
		// Animation Setup
		rotationAnchor = GetNode<Node3D>(Constants.RotationAnchor);
		animatedSprite3D = GetNode<AnimatedSprite3D>(Constants.ActorSpritePath);
		AnimatedSprite3D effectsSprite = GetNode<AnimatedSprite3D>(Constants.EffectsPath);

		if (rotationAnchor == null) GD.PrintErr("[ActorBase._Ready] Could not find an Rotation Anchor for this Actor");
		if (animatedSprite3D == null) GD.PrintErr("[ActorBase._Ready] Could not find an Animated sprite 3D on this Actor");
		if (effectsSprite == null) GD.PrintErr("[ActorBase._Ready] Could not find an Animated sprite 3D for Effects on this Actor");

		actorAnimationHandler = new ActorAnimationHandler(animatedSprite3D) {
			AnimationPrefix = "idle"
		};

		effectAnimationHandler = new EffectAnimationHandler(effectsSprite);

		actorAnimationHandler.ApplyAnimation(inputFaceDirection);
	}
	public PlayerBase GetOwner() {
		GD.Print("[GetOwner] " + Name + " owner is " + GetParent().Name);
		return GetParent<PlayerBase>();
	}

}
