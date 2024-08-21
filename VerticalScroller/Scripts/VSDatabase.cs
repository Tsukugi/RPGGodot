using System.Collections.Generic;
using System.Linq;
using Godot;

public class VSLocalDatabase : LocalDatabase {
    Dictionary<string, VSEnemy> enemies;
    Dictionary<string, VSLevel> levels;

    public VSLocalDatabase() {
        // load base data
        Load<UnitAttributeMutationDTO>("Mutators").ToList().ForEach(mutator => mutators.Add(mutator.Key, mutator.Value));
        // Load custom data
        Loader.UpdateDBPath("./VerticalScroller/Database/");
        enemies = Load<VSEnemy>("Enemies");
        levels = Load<VSLevel>("Level");
        Load<UnitAttributeMutationDTO>("Mutators").ToList().ForEach(mutator => mutators.Add(mutator.Key, mutator.Value));

        GD.Print($"[VSLocalDatabase] Loaded {levels.Count} levels");
        GD.Print($"[VSLocalDatabase] Loaded {enemies.Count} enemies");
        GD.Print($"[VSLocalDatabase] Loaded {mutators.Count} mutators");
    }

    public Dictionary<string, VSEnemy> Enemies => enemies;
    public Dictionary<string, VSLevel> Levels => levels;
}

public class VSEnemy : UnitDTO {
    public string textureName;
}
