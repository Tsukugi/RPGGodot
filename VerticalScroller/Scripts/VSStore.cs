
using System.Collections.Generic;
using Godot;

public partial class VSStore : Store {

    readonly PackedScene mainScene = GD.Load<PackedScene>(VSScenePaths.Main);
    readonly PackedScene storeScene = GD.Load<PackedScene>(VSScenePaths.Store);
    readonly PackedScene gameScene = GD.Load<PackedScene>(VSScenePaths.Game);
    readonly VSMoneyHandler moneyHandler = new();
    VSSaveLoad saveLoad;

    public PackedScene GameScene => gameScene;
    public PackedScene StoreScene => storeScene;
    public PackedScene MainScene => mainScene;
    public VSSaveLoad SaveLoad => saveLoad;
    public VSMoneyHandler MoneyHandler => moneyHandler;

    public override void _Ready() {
        base._Ready();
        saveLoad = new(this);
        saveLoad.RegisterSaveAction(VSSaveModules.MoneyHandler, SerializeForSave);
        saveLoad.LoadGame();
    }

    Dictionary<string, object> SerializeForSave() {
        return new Dictionary<string, object>()   {
            { "Money", moneyHandler.Money }
        };
    }

}

public static class VSSaveModules {
    public const string MoneyHandler = "MoneyHandler";
}