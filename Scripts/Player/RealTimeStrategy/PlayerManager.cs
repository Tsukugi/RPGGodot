
using System.Collections.Generic;
using Godot;
public partial class PlayerManager : Node {
    List<PlayerBase> players = new();
    PlayerRelationship playerRelationship;
    
    List<Node3D> waypoints = new();

    public List<PlayerBase> Players { get => players; }
    public PlayerRelationship PlayerRelationship { get => playerRelationship; }



    public override void _Ready() {
        players = this.TryGetAllChildOfType<PlayerBase>();
        playerRelationship = new(players);
        RealTimeStrategyPlayer player = GetNodeOrNull<RealTimeStrategyPlayer>("Player");
        player.CanvasLayer.Visible = true;
        //! Debug 
        Callable.From(DebugStart).CallDeferred();
        // !EndDebug 
    }


    async void DebugStart() {
        LocalDatabase.LoadData();
        GD.Print("[DebugStart] Loaded " + LocalDatabase.Units.Count + " units");
        GD.Print("[DebugStart] Loaded " + LocalDatabase.Abilites.Count + " abilities");
        GD.Print("[DebugStart] Loaded " + LocalDatabase.Effects.Count + " effects");
        RealTimeStrategyPlayer player = GetNodeOrNull<RealTimeStrategyPlayer>("Player");
        player.AddUnit(LocalDatabase.Units["Tsukugi"], player.GetNodeOrNull<Node3D>("Spawn1").GlobalPosition);

        await Wait(1);
        playerRelationship.UpdateRelationship("Neutral", "Hostile", RelationshipType.Hostile);
        playerRelationship.UpdateRelationship("Hostile", "Neutral", RelationshipType.Hostile);
        playerRelationship.UpdateRelationship("Player", "Hostile", RelationshipType.Hostile);
        playerRelationship.UpdateRelationship("Player", "Neutral", RelationshipType.Friend);
        playerRelationship.UpdateRelationship("Hostile", "Player", RelationshipType.Hostile);

        Node areas = GetNodeOrNull("../Terrain/Areas");
        waypoints = areas.TryGetAllChildOfType<Node3D>();
        RealTimeStrategyPlayer Hostile = GetNodeOrNull<RealTimeStrategyPlayer>("Hostile");
        RealTimeStrategyPlayer Neutral = GetNodeOrNull<RealTimeStrategyPlayer>("Neutral");
        waypoints.Shuffle();

        for (int i = 0; i < waypoints.Count; i++) {
            if (i % 2 == 0) Hostile.AddUnit(LocalDatabase.Units["Tsuki"], waypoints[i].GlobalPosition);
            else Neutral.AddUnit(LocalDatabase.Units["Tsukita"], waypoints[i].GlobalPosition);
        }
        await Wait(1);
        DebugMoveToRandomWaypoints(Neutral, waypoints);
        DebugMoveToRandomWaypoints(Hostile, waypoints);
    }

    void DebugMoveToRandomWaypoints(RealTimeStrategyPlayer player, List<Node3D> waypoints) {
        RealTimeStrategyPlayer firstPlayer = GetNodeOrNull<RealTimeStrategyPlayer>("Player");
        List<NavigationUnit> allUnits = player.GetAllUnits();
        foreach (NavigationUnit unit in allUnits) {

            Color newColor = unit.OverheadLabel.OutlineModulate;
            if (firstPlayer.IsHostilePlayer(player)) newColor = new Color(1, 0, 0);
            if (firstPlayer.GetRelationship(player) == RelationshipType.Friend) {
                newColor = new Color(0.5f, 1, 0.5f);
            }
            if (firstPlayer.GetRelationship(player) == RelationshipType.Neutral) newColor = new Color(0.5f, 0.5f, 0.5f);
            if (firstPlayer.GetRelationship(player) == RelationshipType.Unknown) newColor = new Color(0, 0, 0);
            unit.OverheadLabel.OutlineModulate = newColor;
            firstPlayer.DebugLog(firstPlayer.GetRelationship(player) + " - " + newColor.ToString());

            waypoints.Shuffle();
            Stack<Node3D> WayPoints = new();
            foreach (Node3D waypoint in waypoints) {
                unit.UnitTask.AddTask(new UnitTaskMove(waypoint.GlobalPosition, unit));
            }
        }
    }

    async System.Threading.Tasks.Task Wait(float seconds) {
        await ToSignal(GetTree().CreateTimer(seconds), "timeout");
    }
}



