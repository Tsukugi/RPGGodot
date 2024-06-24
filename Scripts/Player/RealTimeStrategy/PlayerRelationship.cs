
using System.Collections.Generic;
using Godot;

public partial class PlayerRelationship {
    Dictionary<string, Dictionary<string, RelationshipType>> relationships = new();

    public PlayerRelationship(List<PlayerBase> players) {
        foreach (PlayerBase player in players) {
            relationships.Add(player.Name, new Dictionary<string, RelationshipType>());
            foreach (PlayerBase toPlayer in players) {
                relationships[player.Name].Add(toPlayer.Name, RelationshipType.Unknown);
            }
        }
    }

    public void UpdateRelationship(string fromPlayer, string toPlayer, RelationshipType relationship) {
        if (!relationships.ContainsKey(fromPlayer) || !relationships[fromPlayer].ContainsKey(toPlayer)) {
            GD.PrintErr("[UpdateRelationship] No valid players (" + fromPlayer + ", " + toPlayer + ") specified.");
            return;
        }
        relationships[fromPlayer][toPlayer] = relationship;
        GD.Print("[UpdateRelationship] Player " + fromPlayer + " has updated its relationship with " + toPlayer + ". Now its " + relationship);
    }

    public RelationshipType GetRelationship(string fromPlayer, string toPlayer) {
        return relationships[fromPlayer][toPlayer];
    }
}

public enum RelationshipType {
    Hostile, /* Will attack on sight*/
    Neutral, /* Can be interacted with, will ignore you */
    Friend, // ? Can allow more interactions ? */
    Unknown,
}
