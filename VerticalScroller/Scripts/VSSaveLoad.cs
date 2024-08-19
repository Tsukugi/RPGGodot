using System.Collections.Generic;
using System.Linq;
using Godot;

public class VSSaveLoad : SaveLoadBase {
    readonly VSStore store;
    public VSSaveLoad(VSStore store) {
        this.store = store;
    }

    public void LoadGame() {
        var modulesToLoad = LoadSaveFromFile();
        if (modulesToLoad is null) return;
        modulesToLoad.ToList().ForEach(module => {
            GD.Print("[LoadGame] " + module.Key);
            switch (module.Key) {
                case VSSaveModules.MoneyHandler: LoadMoneyHandler(module.Value); break;
            }
        });
    }

    void LoadMoneyHandler(Dictionary<string, object> loadData) {
        int money = (int)(long)loadData["Money"];
        store.MoneyHandler.EarnMoney(money);
        GD.Print("[LoadMoneyHandler] Money amount: " + money);
    }
}