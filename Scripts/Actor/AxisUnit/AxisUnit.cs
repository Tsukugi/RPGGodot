using Godot;

static class AxisNodeNames {
    public static readonly string AttackArea = "AttackArea";
    public static readonly string MeleeCollisionArea = "MeleeCollisionArea";
}
static class AxisNodePaths {
    public static readonly string MeleeAttackArea = "AttackArea";
    public static readonly string Effects = "Effects";
    public static readonly string AURenderer2D = "AURenderer2D";
}

public partial class AxisUnit : Unit {
    readonly AttackHandler attackHandler = new();
    AURenderer2D auRenderer2D = null;
    AxisPlayer player;
    public AURenderer2D Renderer2D { get => auRenderer2D; }
    public new AxisPlayer Player { get => player; }
    public AttackHandler AttackHandler => attackHandler;

    public override void _Ready() {
        base._Ready();
        player = (AxisPlayer)unitPlayerBind.Player;
        // Animation Setup
        auRenderer2D = GetNodeOrNull<AURenderer2D>(AxisNodePaths.AURenderer2D);
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!player.IsFirstPlayer()) return;
        float cameraDegrees = -player.Camera.RotationDegrees.Y;
        Vector2 direction = player.AxisInputHandler.GetRotatedAxis(cameraDegrees);

        switch (player.AxisInputHandler.MovementInputState) {
            case InputState.Stop: {
                    break;
                }
            case InputState.Move: {
                    MoveAndCollide(direction.ToVector3(), (float)delta);
                    break;
                }
        }
        switch (player.AxisInputHandler.ActionInputState) {
            case UnitActionState.Attack: {
                    break;
                }
        }
    }

    public override void _Input(InputEvent @event) {
        if (!player.IsFirstPlayer()) return;
        player.AxisInputHandler.OnInputUpdate();
    }
}
