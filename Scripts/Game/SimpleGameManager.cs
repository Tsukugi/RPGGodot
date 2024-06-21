using Godot;

public static class SimpleGameManager {
    public static readonly string Player = "Player";
    public static readonly string Neutral = "Neutral";
    public static readonly string Hostile = "Hostile";
    public static bool IsFirstPlayerControlled(Node player) {
        return player.Name == Player;
    }

}
