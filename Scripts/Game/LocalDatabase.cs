
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
public class LocalDatabase {
    Dictionary<string, NavigationUnitDTO> units = new();
    Dictionary<string, AbilityDTO> abilites = new();

    public Dictionary<string, AbilityDTO> Abilites { get => abilites; }
    public Dictionary<string, NavigationUnitDTO> Units { get => units; }

    public void LoadData() {
        JSONLoader.LoadUnits().ForEach(unit => units.Add(unit.name, unit));
        JSONLoader.LoadAbilities().ForEach(ability => abilites.Add(ability.name, ability));
    }
}
public static class JSONLoader {
    static readonly string databasePath = "./Database/";
    static List<T> LoadJson<T>(string name) {
        using StreamReader reader = new(databasePath + name + ".json");
        string json = reader.ReadToEnd();
        List<T> items = JsonConvert.DeserializeObject<List<T>>(json);
        return items;
    }

    public static List<NavigationUnitDTO> LoadUnits() {
        return LoadJson<NavigationUnitDTO>("Units");
    }
    public static List<AbilityDTO> LoadAbilities() {
        return LoadJson<AbilityDTO>("Abilities");
    }
}
