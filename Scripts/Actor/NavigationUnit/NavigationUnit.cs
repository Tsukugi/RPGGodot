
using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class NavigationUnit : Unit {
    // Dependencies
    RealTimeStrategyPlayer player;
    AIController aiController;
    UnitCombatArea combatArea;
    UnitAlertArea alertArea;
    UnitNavigationAgent navigationAgent;
    UnitSelection unitSelection;
    UnitTask unitTask;
    public new RealTimeStrategyPlayer Player { get => player; }
    public AIController AiController { get => aiController; }
    public UnitCombatArea CombatArea { get => combatArea; }
    public UnitAlertArea AlertArea { get => alertArea; }
    public UnitNavigationAgent NavigationAgent { get => navigationAgent; }
    public UnitSelection UnitSelection { get => unitSelection; }
    public UnitTask UnitTask { get => unitTask; }

    // State
    UnitRenderDirection unitRenderDirectionState = UnitRenderDirection.Down;

    public override void _Ready() {
        base._Ready();
        player = (RealTimeStrategyPlayer)GetOwner();

        aiController = GetNodeOrNull<AIController>(StaticNodePaths.AIController);
        combatArea = GetNodeOrNull<UnitCombatArea>(StaticNodePaths.CombatArea);
        alertArea = GetNodeOrNull<UnitAlertArea>(StaticNodePaths.AlertArea);
        navigationAgent = GetNodeOrNull<UnitNavigationAgent>(StaticNodePaths.NavigationAgent);
        unitSelection = GetNodeOrNull<UnitSelection>(StaticNodePaths.Selection);
        unitTask = GetNodeOrNull<UnitTask>(StaticNodePaths.TaskController);
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        overheadLabel.Text = Name + " \n " +
            "HP: " + Attributes.HitPoints + " / " + Attributes.MaxHitPoints + " \n " +
            "AlertArea: " + alertArea.AlertState;

       	
    }

    public void NavigateTo(Vector3 direction) {
        Vector3 Velocity = GlobalPosition.DirectionTo(direction) * Attributes.MovementSpeed;
        MoveAndSlide(Velocity);
        UpdateRenderDirection(Velocity.ToVector2());
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
