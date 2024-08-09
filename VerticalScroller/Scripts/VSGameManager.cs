using Godot;
using System;
using System.Collections.Generic;

public partial class VSGameManager : Node {

    readonly PackedScene enemy1 = GD.Load<PackedScene>(VSScenePaths.Enemy1);

    PlayerManager playerManager;
    AxisPlayer enemyPlayer;
    AxisPlayer userPlayer;
    AxisUnit playerUnit;
    List<AxisUnit> enemyUnits = new();


    Timer spawnTimer = new Timer() {
        OneShot = false,
        WaitTime = 1f,
    };

    public override void _Ready() {
        playerManager = GetNode<PlayerManager>(VSNodes.PlayerManager);
        userPlayer = playerManager.GetNode<AxisPlayer>(VSNodes.Player);
        enemyPlayer = playerManager.GetNode<AxisPlayer>(VSNodes.Enemy);
        playerUnit = userPlayer.GetNode<AxisUnit>("PlayerUnit");
        AddChild(spawnTimer);
        spawnTimer.Timeout += OnSpawn;
        spawnTimer.Start();
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        enemyUnits.ForEach(enemyUnit => {
            if (enemyUnit.GlobalPosition.DistanceTo(playerUnit.GlobalPosition) < 10) {
                enemyUnit.MoveTowards(playerUnit.GlobalPosition);
            } else {
                enemyUnit.MoveTowards(Vector3.Back.Magnitude(1000));
            }
        });
    }


    void OnSpawn() {
        AxisUnit enemyUnit = enemy1.Instantiate<AxisUnit>();
        enemyPlayer.AddChild(enemyUnit);
        Node3D spawner1 = enemyPlayer.GetNode<Node3D>("Spawner");
        Node3D spawner2 = enemyPlayer.GetNode<Node3D>("Spawner2");
        List<Node3D> spawners = new() { spawner1, spawner2 };
        spawners.Shuffle();
        enemyUnit.GlobalPosition = spawners[0].GlobalPosition;
        enemyUnits.Add(enemyUnit);
    }
}
