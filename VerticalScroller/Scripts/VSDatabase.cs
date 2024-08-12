using System.Collections.Generic;

public class VSLocalDatabase : LocalDatabase {
    readonly Dictionary<string, VSEnemies> enemies;
    readonly Dictionary<string, VSLevel> levels;

    public VSLocalDatabase() {
        Loader.UpdateDBPath("./VerticalScroller/Database/");
        enemies = Load<VSEnemies>("Enemies");
        levels = Load<VSLevel>("Level");
    }

    public Dictionary<string, VSEnemies> Enemies => enemies;
    public Dictionary<string, VSLevel> Levels => levels;
}

public class VSEnemies : UnitDTO {
    public string textureName;
}