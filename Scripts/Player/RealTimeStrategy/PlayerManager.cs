
using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class PlayerManager : Node {
    List<PlayerBase> players = new();
    PlayerRelationship playerRelationship;

    public List<PlayerBase> Players { get => players; }
    public PlayerRelationship PlayerRelationship { get => playerRelationship; }

    public override void _Ready() {
        players = this.TryGetAllChildOfType<PlayerBase>();
        playerRelationship = new(players);

        //! Debug 
        playerRelationship.UpdateRelationship("Neutral", "Hostile", RelationshipType.Hostile);
        playerRelationship.UpdateRelationship("Hostile", "Neutral", RelationshipType.Hostile);
        playerRelationship.UpdateRelationship("Player", "Hostile", RelationshipType.Hostile);
        playerRelationship.UpdateRelationship("Player", "Neutral", RelationshipType.Friend);


        Node areas = GetNodeOrNull("../NavigationRegion3D/Areas");
        List<Node3D> waypoints = areas.TryGetAllChildOfType<Node3D>();
        RealTimeStrategyPlayer firstPlayer = GetNodeOrNull<RealTimeStrategyPlayer>("Player");

        foreach (PlayerBase _player in players) {
            GD.Print("[PlayerManager._Ready] " + _player.Name);
            if (_player.Name == firstPlayer.Name || _player is not RealTimeStrategyPlayer RTSPlayer) continue;
            List<NavigationUnit> allUnits = RTSPlayer.GetAllUnits();
            foreach (NavigationUnit unit in allUnits) {
                Color newColor = unit.OverheadLabel.OutlineModulate;
                if (firstPlayer.IsHostilePlayer(RTSPlayer)) newColor = new Color(1, 0, 0);
                if (firstPlayer.GetRelationship(RTSPlayer) == RelationshipType.Friend) newColor = new Color(0.5f, 1, 0.5f);
                if (firstPlayer.GetRelationship(RTSPlayer) == RelationshipType.Neutral) newColor = new Color(0.5f, 0.5f, 0.5f);
                if (firstPlayer.GetRelationship(RTSPlayer) == RelationshipType.Unknown) newColor = new Color(0, 0, 0);
                unit.OverheadLabel.OutlineModulate = newColor;
                firstPlayer.DebugLog(firstPlayer.GetRelationship(RTSPlayer) + " - " + newColor.ToString());


                waypoints.Shuffle();
                Stack<Node3D> WayPoints = new();
                foreach (Node3D waypoint in waypoints) {
                    unit.UnitTask.Add(new UnitTaskMove(waypoint.GlobalPosition, unit));
                }
            }
        }

    }
    // !EndDebug 
    //! DebugEnd

}



