using System.Collections.Generic;

public class VSLocalDatabase : LocalDatabase {
    readonly Dictionary<string, VSEnemy> enemies;
    readonly Dictionary<string, VSLevel> levels;

    public VSLocalDatabase() {
        Loader.UpdateDBPath("./VerticalScroller/Database/");
        enemies = Load<VSEnemy>("Enemies");
        levels = Load<VSLevel>("Level");
    }

    public Dictionary<string, VSEnemy> Enemies => enemies;
    public Dictionary<string, VSLevel> Levels => levels;
}

public class VSEnemy : UnitDTO {
    public string textureName;
}
