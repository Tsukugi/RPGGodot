using Godot;

public partial class RTSNavigation : Node {
    RealTimeStrategyPlayer player;

    // Navigation
    float navigationGroupGapDistance = 1;
    public readonly NavigationInputHandler NavigationInputHandler = new();
    public readonly NavigationBase NavigationBase = new();

    public override void _Ready() {
        base._Ready();
        player = this.TryFindParentNodeOfType<RealTimeStrategyPlayer>();
    }

    public override void _Input(InputEvent @event) {
        base._Input(@event);
        if (!player.IsFirstPlayer()) return;
        if (@event is InputEventMouseButton eventMouseButton) {
            if (eventMouseButton.ButtonIndex == MouseButton.Right) {
                if (eventMouseButton.Pressed) {
                    Vector3? targetPosition = NavigationBase.GetNavigationTargetPosition(player.Camera);
                    if (targetPosition is not Vector3 targetPositionInWorld) return;
                    SelectionUtils.ApplyCommandToGroupPosition(
                        player.RTSSelection.SelectedUnits,
                        targetPositionInWorld,
                        navigationGroupGapDistance,
                        (float)System.Math.Floor(System.Math.Sqrt(player.RTSSelection.SelectedUnits.Count)),
                        ApplyNavigation);
                }
            }
        }
    }

    static void ApplyNavigation(NavigationUnit unit, Vector3 targetPosition) {
        // TODO Improve me to accumulate tasks instead
        if (unit.UnitSelection.IsSelected) {
            unit.NavigationAgent.StartNewNavigation(targetPosition);
        } else {
            unit.UnitTask.AddTask(new UnitTaskMove(targetPosition, unit));
        }
    }
}