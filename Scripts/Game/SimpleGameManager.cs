public static class SimpleGameManager {
    public static readonly string Player = "Player";
    public static readonly string Neutral = "Neutral";
    public static readonly string Hostile = "Hostile";
    public static readonly string Environment = "Environment";
    public static bool IsFirstPlayer(this PlayerBase player) {
        return player.Name == Player;
    }
}
