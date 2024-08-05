
using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class NavigationUnit : Unit {
    // Dependencies
    RealTimeStrategyPlayer player;
    AIController aiController;
    UnitCombat combatArea;
    UnitAlertArea alertArea;
    UnitNavigationAgent navigationAgent;
    UnitSelection unitSelection;
    UnitTask unitTask;
    ShapeCast3D detectionCast;
    public new RealTimeStrategyPlayer Player { get => player; }
    public AIController AiController { get => aiController; }
    public UnitCombat UnitCombat { get => combatArea; }
    public UnitAlertArea AlertArea { get => alertArea; }
    public UnitNavigationAgent NavigationAgent { get => navigationAgent; }
    public UnitSelection UnitSelection { get => unitSelection; }
    public UnitTask UnitTask { get => unitTask; }
    public ShapeCast3D DetectionCast { get => detectionCast; }

    // State
    UnitRenderDirection unitRenderDirectionState = UnitRenderDirection.Down;

    public override void _Ready() {
        base._Ready();
        player = (RealTimeStrategyPlayer)GetOwner();

        aiController = GetNode<AIController>(StaticNodePaths.AIController);
        combatArea = GetNode<UnitCombat>(StaticNodePaths.Combat);
        alertArea = GetNode<UnitAlertArea>(StaticNodePaths.AlertArea);
        navigationAgent = GetNode<UnitNavigationAgent>(StaticNodePaths.NavigationAgent);
        unitSelection = GetNode<UnitSelection>(StaticNodePaths.Selection);
        unitTask = GetNode<UnitTask>(StaticNodePaths.TaskController);
        detectionCast = GetNode<ShapeCast3D>(StaticNodePaths.TaskController_DetectionCast);

        Attributes.OnKilled += OnKilledHandler;
    }

    void OnKilledHandler(Unit unit) {
        navigationAgent.CancelNavigation();
        combatArea.EndCombat();
        alertArea.SetMonitoringEnabled(false);
        unitTask.ClearAll();
    }

    public void NavigateTo(Vector3 direction) {
        Vector3 Velocity = GlobalPosition.DirectionTo(direction) * Attributes.MovementSpeed;
        MoveAndSlide(Velocity);
        UpdateRenderDirection(Velocity.ToVector2().Rotate(RotationDegrees.X));
    }

    void UpdateRenderDirection(Vector2 direction) {
        UnitRenderDirection newUnitDirection = ActorAnimationHandler.GetRenderDirectionFromVector(direction);
        if (newUnitDirection != unitRenderDirectionState) {
            unitRenderDirectionState = newUnitDirection;
            ActorAnimationHandler.ApplyAnimation(unitRenderDirectionState);
        }
    }
}

public static class NavigationUnitUtils {
    public static List<NavigationUnit> FilterNavigationUnits(this Array<Node3D> array) {
        List<NavigationUnit> navUnits = new();
        foreach (Node3D node in array) {
            if (node is NavigationUnit unit) navUnits.Add(unit);
        }
        return navUnits;
    }
}
