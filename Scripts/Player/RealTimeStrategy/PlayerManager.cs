
using System.Collections.Generic;
using Godot;
public partial class PlayerManager : Node {
    List<PlayerBase> players = new();
    PlayerRelationship playerRelationship;

    public List<PlayerBase> Players { get => players; }
    public PlayerRelationship PlayerRelationship { get => playerRelationship; }

    public override void _Ready() {
        players = this.TryGetAllChildOfType<PlayerBase>();
        playerRelationship = new(players);
        //! Debug 
        Callable.From(DebugStart).CallDeferred();
        // !EndDebug 
    }


    async void DebugStart() {
        await Wait(1);
        playerRelationship.UpdateRelationship("Neutral", "Hostile", RelationshipType.Hostile);
        playerRelationship.UpdateRelationship("Hostile", "Neutral", RelationshipType.Hostile);
        playerRelationship.UpdateRelationship("Player", "Hostile", RelationshipType.Hostile);
        playerRelationship.UpdateRelationship("Player", "Neutral", RelationshipType.Friend);
        playerRelationship.UpdateRelationship("Hostile", "Player", RelationshipType.Hostile);

        Node areas = GetNodeOrNull("../NavigationRegion3D/Areas");
        List<Node3D> waypoints = areas.TryGetAllChildOfType<Node3D>();
        RealTimeStrategyPlayer Hostile = GetNodeOrNull<RealTimeStrategyPlayer>("Hostile");
        RealTimeStrategyPlayer Neutral = GetNodeOrNull<RealTimeStrategyPlayer>("Neutral");
        waypoints.Shuffle();
        Hostile.AddUnits(waypoints[0].GlobalPosition.WithY(0), 3);
        Neutral.AddUnits(waypoints[1].GlobalPosition.WithY(0), 3);
        Hostile.AddUnits(waypoints[^1].GlobalPosition.WithY(0), 3);
        Neutral.AddUnits(waypoints[^2].GlobalPosition.WithY(0), 3);
        waypoints.Shuffle();
        Hostile.AddUnits(waypoints[0].GlobalPosition.WithY(0), 3);
        Neutral.AddUnits(waypoints[1].GlobalPosition.WithY(0), 3);
        Hostile.AddUnits(waypoints[^1].GlobalPosition.WithY(0), 3);
        Neutral.AddUnits(waypoints[^2].GlobalPosition.WithY(0), 3);

        await Wait(1);
        DebugMoveToRandomWaypoints(Neutral, waypoints);
        DebugMoveToRandomWaypoints(Hostile, waypoints);
    }


    void DebugMoveToRandomWaypoints(RealTimeStrategyPlayer player, List<Node3D> waypoints) {
        RealTimeStrategyPlayer firstPlayer = GetNodeOrNull<RealTimeStrategyPlayer>("Player");
        List<NavigationUnit> allUnits = player.GetAllUnits();
        foreach (NavigationUnit unit in allUnits) {
            Color newColor = unit.OverheadLabel.OutlineModulate;
            // if (firstPlayer.IsHostilePlayer(player)) unit.AlertArea.AlertStateOnEnemySight = AlertState.Safe;
            if (firstPlayer.IsHostilePlayer(player)) newColor = new Color(1, 0, 0);
            if (firstPlayer.GetRelationship(player) == RelationshipType.Friend) {
                newColor = new Color(0.5f, 1, 0.5f);
                unit.Attributes.Update(10000, 10, 1);
            }
            if (firstPlayer.GetRelationship(player) == RelationshipType.Neutral) newColor = new Color(0.5f, 0.5f, 0.5f);
            if (firstPlayer.GetRelationship(player) == RelationshipType.Unknown) newColor = new Color(0, 0, 0);
            unit.OverheadLabel.OutlineModulate = newColor;
            firstPlayer.DebugLog(firstPlayer.GetRelationship(player) + " - " + newColor.ToString());

            waypoints.Shuffle();
            Stack<Node3D> WayPoints = new();
            foreach (Node3D waypoint in waypoints) {
                unit.UnitTask.Add(new UnitTaskMove(waypoint.GlobalPosition, unit));
            }
        }
    }

    async System.Threading.Tasks.Task Wait(float seconds) {
        await ToSignal(GetTree().CreateTimer(seconds), "timeout");
    }
}



