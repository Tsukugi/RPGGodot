using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class VSGameManager : Node {
    VSLocalDatabase vsLocalDatabase;
    [Export]
    string currentLevel = "Level1";
    [Export]
    int currentEncounterIndex = 0;

    readonly PackedScene enemyScene = GD.Load<PackedScene>(VSScenePaths.Enemy1);
    readonly PackedScene chestScene = GD.Load<PackedScene>(VSScenePaths.Chest);

    PlayerManager playerManager;
    AxisPlayer enemyPlayer;
    AxisPlayer userPlayer;
    AxisUnit playerUnit;
    readonly Dictionary<string, AxisUnit> enemyUnits = new();

    Timer spawnTimer = new() {
        OneShot = false,
        WaitTime = 5f,
    };

    public VSLocalDatabase Database { get => vsLocalDatabase; }

    public override void _Ready() {
        vsLocalDatabase = new VSLocalDatabase();
        playerManager = GetNode<PlayerManager>(VSNodes.PlayerManager);
        userPlayer = playerManager.GetNode<AxisPlayer>(VSNodes.Player);
        enemyPlayer = playerManager.GetNode<AxisPlayer>(VSNodes.Enemy);
        playerUnit = userPlayer.GetNode<AxisUnit>("PlayerUnit");
        AddChild(spawnTimer);
        spawnTimer.Timeout += OnSpawn;
        spawnTimer.Start();
        OnSpawn(); // Start immediately
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        enemyUnits.Values.ToList().ForEach(enemyUnit => {
            if (enemyUnit.GlobalPosition.DistanceTo(playerUnit.GlobalPosition) < 3) {
                enemyUnit.MoveTowards(playerUnit.GlobalPosition);
            } else {
                enemyUnit.MoveTowards(enemyUnit.GlobalPosition.WithZ(1000));
            }
        });
    }


    void OnSpawn() {
        if (vsLocalDatabase.Levels[currentLevel].encounters.Count <= currentEncounterIndex) {
            GD.Print("[OnSpawn] Level complete");
            return;
        }
        List<VSEncounter> encounters = vsLocalDatabase.Levels[currentLevel].encounters[currentEncounterIndex];

        List<Node3D> spawners = GetRandomSpawners();
        for (int i = 0; i < spawners.Count; i++) {
            HandleEncounter(encounters[i], spawners[i].GlobalPosition);
        }

        currentEncounterIndex++;
    }

    void HandleEncounter(VSEncounter? encounter, Vector3 spawnPosition) {
        if (encounter is null) return;
        GD.Print("[HandleEncounter] Encounter " + encounter.type);
        switch (encounter.type) {
            case VSEncounterTypes.Enemy:
                for (int i = 0; i < encounter.enemyEncounter.nOfEnemies; i++) {
                    SpawnEnemy(encounter.enemyEncounter.unitType, spawnPosition);
                }
                break;
            case VSEncounterTypes.Chest:
                SpawnChest(encounter, spawnPosition);
                // TODO spawn chest and give it an id
                encounter.chestEncounter.rewards.ForEach(reward => GiveReward(reward));
                break;
        }
    }

    void GiveReward(VSReward reward) {
        switch (reward.type) {
            case VSRewardTypes.Buff:
                GD.Print("[GiveReward] We give an unimplemented buff");
                break;
            case VSRewardTypes.Money:
                GD.Print("[GiveReward] We give " + reward.moneyAmount + " money");
                break;
        }
    }

    // TODO Remove duplicate code
    void SpawnChest(VSEncounter encounter, Vector3 position) {
        VSUnit chestUnit = chestScene.Instantiate<VSUnit>();
        enemyPlayer.AddChild(chestUnit);
        chestUnit.UnitAttributes.ApplyHeal(encounter.chestEncounter.hp);
        chestUnit.GlobalPosition = position;
        chestUnit.UnitAttributes.OnKilled += OnEnemyUnitKilled;
        AttributesExport chestUnitAttributes = chestUnit.GetAttributes();
        chestUnit.OverheadLabel.Text = chestUnitAttributes.HitPoints + " / " + chestUnitAttributes.MaxHitPoints;

        GD.Print("[SpawnChest] Spawn " + chestUnit.Name + " as chest");
        GD.Print("[SpawnChest] Enemy units handled: " + enemyUnits.Count);

        enemyUnits.Add(chestUnit.Name, chestUnit);
    }

    void SpawnEnemy(string unitId, Vector3 position) {
        VSUnit enemyUnit = enemyScene.Instantiate<VSUnit>();
        enemyPlayer.AddChild(enemyUnit);
        enemyUnit.UpdateUnit(Database.Enemies[unitId]);
        enemyUnit.GlobalPosition = position;
        enemyUnit.UnitAttributes.OnKilled += OnEnemyUnitKilled;
        AttributesExport enemyUnitAttributes = enemyUnit.GetAttributes();
        enemyUnit.OverheadLabel.Text = enemyUnitAttributes.HitPoints + " / " + enemyUnitAttributes.MaxHitPoints;

        GD.Print("[SpawnEnemy] Spawn " + enemyUnit.Name + " as enemy");
        GD.Print("[SpawnEnemy] Enemy units handled: " + enemyUnits.Count);

        enemyUnits.Add(enemyUnit.Name, enemyUnit);
    }

    void OnEnemyUnitKilled(Unit dyingUnit) {
        enemyUnits.Remove(dyingUnit.Name);
        dyingUnit.QueueFree();
    }

    List<Node3D> GetRandomSpawners() {
        Node3D spawner1 = enemyPlayer.GetNode<Node3D>("Spawner");
        Node3D spawner2 = enemyPlayer.GetNode<Node3D>("Spawner2");
        List<Node3D> spawners = new() { spawner1, spawner2 };
        return spawners.ShuffleNew();
    }
}
