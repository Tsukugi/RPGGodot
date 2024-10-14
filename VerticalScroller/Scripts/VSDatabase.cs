using System.Collections.Generic;
using System.Linq;
using Godot;

public class VSLocalDatabase : LocalDatabase {
    readonly Dictionary<string, VSEnemy> enemies = new();
    readonly Dictionary<string, VSLevel> levels = new();
    readonly Dictionary<string, MutationDTO> playerMutators = new();

    public VSLocalDatabase() {
        // load base data
        Load<MutationDTO>("Mutators").ToList().ForEach(mutator => unitMutators.Add(mutator.Key, mutator.Value));
        // Load custom data
        Loader.UpdateDBPath("./VerticalScroller/Database/");
        enemies = Load<VSEnemy>("Enemies");
        levels = Load<VSLevel>("Level");
        Load<MutationDTO>("Mutators").ToList().ForEach(mutator => unitMutators.Add(mutator.Key, mutator.Value));
        Load<MutationDTO>("PlayerMutators").ToList().ForEach(mutator => playerMutators.Add(mutator.Key, mutator.Value));

        GD.Print($"[VSLocalDatabase] Loaded {levels.Count} levels");
        GD.Print($"[VSLocalDatabase] Loaded {enemies.Count} enemies");
        GD.Print($"[VSLocalDatabase] Loaded {unitMutators.Count} mutators");
        GD.Print($"[VSLocalDatabase] Loaded {playerMutators.Count} player mutators");
    }

    public Dictionary<string, VSEnemy> Enemies => enemies;
    public Dictionary<string, VSLevel> Levels => levels;
    public Dictionary<string, MutationDTO> PlayerMutators => playerMutators;
}

public class VSEnemy : UnitDTO {
    public string textureName;
}
