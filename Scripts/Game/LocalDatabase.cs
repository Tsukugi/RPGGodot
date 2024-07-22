using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
public class LocalDatabase {
    Dictionary<string, NavigationUnitDTO> units = new();
    Dictionary<string, AbilityDTO> abilites = new();
    Dictionary<string, EffectBaseDTO> effects = new();

    public Dictionary<string, AbilityDTO> Abilites { get => abilites; }
    public Dictionary<string, NavigationUnitDTO> Units { get => units; }
    public Dictionary<string, EffectBaseDTO> Effects { get => effects; }

    public void LoadData() {
        JSONLoader.LoadUnits().ForEach(unit => units.Add(unit.name, unit));
        JSONLoader.LoadAbilities().ForEach(ability => abilites.Add(ability.name, ability));
        JSONLoader.LoadEffects().ForEach(effect => effects.Add(effect.id, effect));
    }
}
public static class JSONLoader {
    static readonly string databasePath = "./Database/";

    static string LoadJson(string name) {
        using StreamReader reader = new(databasePath + name + ".json");
        string json = reader.ReadToEnd();
        return json;
    }
    static List<T> GetListFromJson<T>(string name) {
        string json = LoadJson(name);
        List<T> items = JsonConvert.DeserializeObject<List<T>>(json);
        return items;
    }

    public static List<NavigationUnitDTO> LoadUnits() {
        return GetListFromJson<NavigationUnitDTO>("Units");
    }
    public static List<AbilityDTO> LoadAbilities() {
        return GetListFromJson<AbilityDTO>("Abilities");
    }
    public static List<EffectBaseDTO> LoadEffects() {
        return GetListFromJson<EffectBaseDTO>("Effects");
    }
}
