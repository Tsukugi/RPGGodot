using System.Collections.Generic;
using System.IO;
using Godot;
using Newtonsoft.Json;
public class LocalDatabase {
    Dictionary<string, UnitDTO> units = new();
    Dictionary<string, AbilityDTO> abilites = new();
    Dictionary<string, EffectBaseDTO> effects = new();

    JSONLoader loader = new();

    public Dictionary<string, AbilityDTO> Abilites { get => abilites; }
    public Dictionary<string, UnitDTO> Units { get => units; }
    public Dictionary<string, EffectBaseDTO> Effects { get => effects; }
    public JSONLoader Loader { get => loader; }

    public void LoadData() {
        LoadUnits().ForEach(unit => units.Add(unit.name, unit));
        LoadAbilities().ForEach(ability => abilites.Add(ability.name, ability));
        LoadEffects().ForEach(effect => effects.Add(effect.id, effect));
    }

    public EffectBaseDTO GetEffect(string key) {
        return effects[key];
    }

    public Dictionary<string, T> Load<T>(string name) where T : DTOBase {
        Dictionary<string, T> values = new();
        loader.GetListFromJson<T>(name).ForEach(value => values.Add(value.id, value));
        return values;
    }

    public List<UnitDTO> LoadUnits() {
        return loader.GetListFromJson<UnitDTO>("Units");
    }
    public List<AbilityDTO> LoadAbilities() {
        return loader.GetListFromJson<AbilityDTO>("Abilities");
    }
    public List<EffectBaseDTO> LoadEffects() {
        return loader.GetListFromJson<EffectBaseDTO>("Effects");
    }
}

public class JSONLoader {
    string databasePath = "./Database/";


    string LoadJson(string name) {
        using StreamReader reader = new(databasePath + name + ".json");
        string json = reader.ReadToEnd();
        return json;
    }
    public List<T> GetListFromJson<T>(string name) {
        string json = LoadJson(name);
        List<T> items = JsonConvert.DeserializeObject<List<T>>(json);
        return items;
    }

    public void UpdateDBPath(string newPath) {
        GD.Print("[UpdateDBPath] New path is " + newPath);
        databasePath = newPath;
    }
}
