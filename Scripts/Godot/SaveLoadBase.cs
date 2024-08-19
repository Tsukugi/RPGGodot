using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;

public class SaveLoadBase {
    public delegate Dictionary<string, object> SaveAction();
    readonly Dictionary<string, SaveAction> saveActions = new();

    public void SaveGame() {
        Dictionary<string, Dictionary<string, object>> preparedData = new();
        foreach (var item in saveActions) {
            // Call all the actions to retrieve their data
            preparedData.Add(item.Key, item.Value());
        }
        SaveActionsToFile(preparedData);
    }

    public void RegisterSaveAction(string name, SaveAction action) {
        saveActions.Add(name, action);
    }


    // * This gets the loaded data from JSON, you still need to implement how to load it.
    public Dictionary<string, Dictionary<string, object>> LoadSaveFromFile() {
        if (!FileAccess.FileExists("user://savegame.save")) {
            return null; // Error! We don't have a save to load.
        }

        using var saveFile = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Read);

        var jsonString = saveFile.GetLine();

        try {
            var result = JSONLoader.Deserialize<Dictionary<string, object>>(jsonString);

            if (result is Dictionary<string, Dictionary<string, object>> validResult) return validResult;

            return null;
        } catch (System.Exception e) {
            GD.PrintErr(e);
            return null;
        }

    }

    void SaveActionsToFile(Dictionary<string, Dictionary<string, object>> saveActions) {
        using var saveFile = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Write);

        // Json provides a static method to serialized JSON string.
        string jsonString = JsonConvert.SerializeObject(saveActions);

        // Store the save dictionary as a new line in the save file.
        saveFile.StoreLine(jsonString);
    }
}