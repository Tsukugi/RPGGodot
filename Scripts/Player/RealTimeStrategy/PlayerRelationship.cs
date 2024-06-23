using Godot;
using Godot.Collections;

public partial class PlayerRelationship {
    Dictionary<string, Dictionary<string, RelationshipType>> relationships = new();

    public void Add(string fromPlayer, string toPlayer, RelationshipType relationship) {
        relationships[fromPlayer][toPlayer] = relationship;
        GD.Print(fromPlayer, toPlayer, relationship);
    }
}
public enum RelationshipType {
    Hostile, /* Will attack on sight*/
    Neutral, /* Can be interacted with, will ignore you */
    Friend, // ? Can allow more interactions ? */
}