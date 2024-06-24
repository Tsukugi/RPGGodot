
using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class PlayerManager : Node {
    List<PlayerBase> players = new();
    PlayerRelationship playerRelationship;

    public List<PlayerBase> Players { get => players; }
    public PlayerRelationship PlayerRelationship { get => playerRelationship; }

    public override void _Ready() {
        Array<Node> children = GetChildren();
        foreach (Node child in children) {
            if (child is not PlayerBase player) return;
            players.Add(player);
        }
        playerRelationship = new(players);

        //! Debug 
        playerRelationship.UpdateRelationship("Neutral", "Hostile", RelationshipType.Hostile);
        playerRelationship.UpdateRelationship("Hostile", "Neutral", RelationshipType.Hostile);
        playerRelationship.UpdateRelationship("Player", "Hostile", RelationshipType.Hostile);
        playerRelationship.UpdateRelationship("Player", "Neutral", RelationshipType.Friend);

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
            }
        }
        //! DebugEnd

    }
}


