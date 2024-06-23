
using Godot;
using Godot.Collections;

public partial class PlayerManager : Node {
    Dictionary<string, PlayerBase> players = new();
    PlayerRelationship playerRelationship = new();

    public Dictionary<string, PlayerBase> Players { get => players; }
    public PlayerRelationship PlayerRelationship { get => playerRelationship; }

    public override void _Ready() {
        Array<Node> children = GetChildren();
        foreach (Node child in children) {
            if (child is not PlayerBase player) return;
            players.Add(player.Name, player);
        }

        //! Debug 
        PlayerRelationship.Add()
        //! DebugEnd
    }
}


