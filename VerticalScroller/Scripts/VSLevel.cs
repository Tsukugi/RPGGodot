using System.Collections.Generic;

public class VSLevel : DTOBase {
    public List<List<VSEncounter>> encounters = new(); // We use a list of encounters per spawn [[ spawn1Encounter1, spawn2Encounter1 ], [ spawn1Encounter2, spawn2Encounter2]];
}

public class VSEncounter {
    public string type; // VSEncounterTypes
    public VSEnemyEncounter enemyEncounter;
    public VSChestEncounter chestEncounter;
}

public class VSRewardableEncounter {
    public List<VSReward> rewards;
}
public class VSEnemyEncounter : VSRewardableEncounter {
    public int nOfEnemies;
    public string unitType;
}
public class VSChestEncounter : VSRewardableEncounter {
    public int hp;
}

public class VSReward {
    public string type; // VSRewardTypes
    public int moneyAmount;
}

public static class VSEncounterTypes {
    public const string Enemy = "Enemy";
    public const string Chest = "Chest";
}
public static class VSRewardTypes {
    public const string Money = "Money";
}
