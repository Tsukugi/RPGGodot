using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public partial class VSGameManager : Node {
    protected VSStore store;
    VSLocalDatabase vsLocalDatabase;
    [Export]
    string currentLevel = "Level1";
    [Export]
    int currentEncounterIndex = 0;

    readonly PackedScene enemyScene = GD.Load<PackedScene>(VSScenePaths.Enemy);
    readonly PackedScene chestScene = GD.Load<PackedScene>(VSScenePaths.Chest);

    PlayerManager playerManager;
    AxisPlayer enemyPlayer;
    AxisPlayer userPlayer;
    VSUIGame playerUI;
    readonly List<VSUIMutatorSelector> mutatorSelectors = new();

    AxisUnit playerUnit;

    Timer encounterTimer = new() {
        OneShot = false,
        WaitTime = 5f,
    };

    public VSLocalDatabase Database { get => vsLocalDatabase; }
    public AxisUnit PlayerUnit { get => playerUnit; }

    public override void _Ready() {
        store = GetNode<VSStore>(VSNodes.Store);
        vsLocalDatabase = new VSLocalDatabase();
        playerManager = GetNode<PlayerManager>(VSNodes.PlayerManager);
        userPlayer = playerManager.GetNode<AxisPlayer>(VSNodes.Player);
        enemyPlayer = playerManager.GetNode<AxisPlayer>(VSNodes.Enemy);
        playerUnit = userPlayer.GetNode<AxisUnit>("PlayerUnit");
        playerUI = userPlayer.GetNode<VSUIGame>("CanvasLayer/Control");
        var mutatorChildren = playerUI.GetNode("MutatorSelectionContainer").GetChildren();
        foreach (var node in mutatorChildren) {
            if (node is not VSUIMutatorSelector selector) continue;
            mutatorSelectors.Add(selector);
            selector.OnMutatorSelected -= OnSelectMutation;
            selector.OnMutatorSelected += OnSelectMutation;
        }

        // Encounter Timer
        AddChild(encounterTimer);
        encounterTimer.Timeout += OnEncounter;
        encounterTimer.Start();
        OnEncounter(); // Start immediately

        // Server
        userPlayer.PlayerWSBind.AddPlayer();
        playerUnit.UnitWSBind.RegisterUnit();
        playerUnit.UnitWSBind.CalculateMutations();
        playerUnit.UnitWSBind.GetMutatedAttributes();
    }

    void OnGameEnd() {
        GetTree().ChangeSceneToPacked(store.MainScene);
    }

    void OnEncounter() {
        if (vsLocalDatabase.Levels[currentLevel].encounters.Count <= currentEncounterIndex) {
            GD.Print("[OnEncounter] Level complete");
            encounterTimer.Stop();
            store.SaveLoad.SaveGame();
            OnGameEnd();
            return;
        }

        List<VSEncounter> encounters = vsLocalDatabase.Levels[currentLevel].encounters[currentEncounterIndex];
        GD.Print("[OnEncounter] Encounter " + currentEncounterIndex);

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
                    SpawnEnemy(encounter, spawnPosition);
                }
                break;
            case VSEncounterTypes.Chest:
                SpawnChest(encounter, spawnPosition);
                break;
        }
    }

    static string GetRandKey(List<string> keyList) => keyList[new Random().Next(keyList.Count)];
    MutationDTO GetRandomMutator() {
        int randNumber = new Random().Next(100);
        // ! Simple random pick
        // TODO change this if we support more mutationTypes
        if (randNumber < 50) {
            List<string> keyList = new(vsLocalDatabase.UnitMutators.Keys);
            var result = vsLocalDatabase.UnitMutators[GetRandKey(keyList)];
            result.mutationTarget = MutationTargets.Unit;
            return result;
        } else {
            List<string> keyList = new(vsLocalDatabase.PlayerMutators.Keys);
            var result = vsLocalDatabase.PlayerMutators[GetRandKey(keyList)];
            result.mutationTarget = MutationTargets.Player;
            return result;
        }
    }

    void BuildMutatorSelectors() {
        foreach (var node in mutatorSelectors) {
            if (node is not VSUIMutatorSelector mutatorSelector) continue;
            var positive = GetRandomMutator();
            var negative = GetRandomMutator();
            mutatorSelector.UpdateMutations(positive, negative);
        }
    }

    void OnSelectMutation(MutationDTO positive, MutationDTO negative) {
        playerUI.SetMutatorSelectionVisibility(false);

        switch (positive.mutationTarget) {
            case MutationTargets.Player: playerUnit.Player.PlayerWSBind.AddMutation(positive); break;
            case MutationTargets.Unit: playerUnit.UnitAttributes.AddMutation(positive); break;
            default: GD.PrintErr("No mutationType"); break;
        }
        switch (negative.mutationTarget) {
            case MutationTargets.Player: playerUnit.Player.PlayerWSBind.AddMutation(negative); break;
            case MutationTargets.Unit: playerUnit.UnitAttributes.AddMutation(negative); break;
            default: GD.PrintErr("No mutationType"); break;
        }
    }

    void GiveReward(VSReward reward) {
        switch (reward.type) {
            case VSRewardTypes.Money:
                store.MoneyHandler.EarnMoney(reward.moneyAmount);
                GD.Print("[GiveReward] We give " + reward.moneyAmount + " money. Money amount: " + store.MoneyHandler.Money);
                break;
        }
    }

    void GiveRewards(VSEncounter encounter) {
        switch (encounter.type) {
            case VSEncounterTypes.Enemy:
                encounter.enemyEncounter.rewards.ForEach(reward => GiveReward(reward));
                break;
            case VSEncounterTypes.Chest:
                encounter.chestEncounter.rewards.ForEach(reward => GiveReward(reward));
                BuildMutatorSelectors();
                playerUI.SetMutatorSelectionVisibility(true);
                GD.Print("[GiveReward] Trigger mutator selection");
                break;
        }
    }

    void SpawnChest(VSEncounter encounter, Vector3 position) {
        VSUnit chestUnit = chestScene.Instantiate<VSUnit>();
        enemyPlayer.AddChild(chestUnit);
        chestUnit.UnitAttributes.SetMaxHitPoints(encounter.chestEncounter.hp);

        chestUnit.GlobalPosition = position;
        chestUnit.UnitAttributes.OnKilled += (Unit dyingUnit) => GiveRewards(encounter);

        AttributesExport chestUnitAttributes = chestUnit.GetAttributes();
        chestUnit.OverheadLabel.Text = chestUnitAttributes.HitPoints + "";

        GD.Print("[SpawnChest] Spawn " + chestUnit.Name + " as chest");
    }

    void SpawnEnemy(VSEncounter encounter, Vector3 position) {
        VSUnit enemyUnit = enemyScene.Instantiate<VSUnit>();
        enemyPlayer.AddChild(enemyUnit);
        enemyUnit.UpdateUnit(Database.Enemies[encounter.enemyEncounter.unitType]);

        enemyUnit.GlobalPosition = position;
        enemyUnit.UnitAttributes.OnKilled += (Unit dyingUnit) => GiveRewards(encounter);

        AttributesExport enemyUnitAttributes = enemyUnit.GetAttributes();
        enemyUnit.OverheadLabel.Text = enemyUnitAttributes.HitPoints + "";
        GD.Print("[SpawnEnemy] Spawn " + enemyUnit.Name + " as enemy");
    }

    List<Node3D> GetRandomSpawners() {
        Node3D spawner1 = enemyPlayer.GetNode<Node3D>("Spawner");
        Node3D spawner2 = enemyPlayer.GetNode<Node3D>("Spawner2");
        List<Node3D> spawners = new() { spawner1, spawner2 };
        return spawners.ShuffleNew();
    }
}
