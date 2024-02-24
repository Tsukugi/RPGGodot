
using Godot;
public partial class ActorBase : CharacterBody3D {
    [Export] // Initial animation direction
    public InputFaceDirection inputFaceDirection = InputFaceDirection.Down;

    private AnimatedSprite3D animatedSprite3D = null;
    private ActorAnimationHandler actorAnimationHandler = null;
    private EffectAnimationHandler effectAnimationHandler = null;
    private Area3D talkArea = null;
    private Node3D rotationAnchor = null;
    private PlayerBase player = null;

    protected PlayerBase Player { get => player; }
    protected ActorAnimationHandler ActorAnimationHandler { get => actorAnimationHandler; }
    protected EffectAnimationHandler EffectAnimationHandler { get => effectAnimationHandler; }
    protected Area3D TalkArea { get => talkArea; }
    protected Node3D RotationAnchor { get => rotationAnchor; }

    public override void _Ready() {
        player = GetOwner();

        talkArea = GetNode<Area3D>("TalkArea");

        talkArea.AreaEntered += OnInteractionAreaEnteredHandler;
        talkArea.AreaExited += OnInteractionAreaExitedHandler;

        // Animation Setup
        rotationAnchor = GetNode<Node3D>("RotationAnchor");
        animatedSprite3D = GetNode<AnimatedSprite3D>("StaticRotation/AnimatedSprite3D");
        AnimatedSprite3D effectsSprite = GetNode<AnimatedSprite3D>("RotationAnchor/Effects");

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


    void OnInteractionAreaEnteredHandler(Area3D area) {
        if (!SimpleGameManager.IsFirstPlayerControlled(player)) return;
        player.InteractionPanel.Message.Text = "Talk to " + area.GetParent().Name;
        player.InteractionPanel.Visible = true;
    }
    void OnInteractionAreaExitedHandler(Area3D area) {
        if (!SimpleGameManager.IsFirstPlayerControlled(player)) return;
        player.InteractionPanel.Visible = false;
    }
}
