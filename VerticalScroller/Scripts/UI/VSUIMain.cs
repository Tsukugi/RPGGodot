using Godot;

public partial class VSUIMain : Node {

    readonly PackedScene mainScene = GD.Load<PackedScene>(VSScenePaths.Main);
    readonly PackedScene storeScene = GD.Load<PackedScene>(VSScenePaths.Store);
    readonly PackedScene gameScene = GD.Load<PackedScene>(VSScenePaths.Game);

    public PackedScene GameScene => gameScene;
    public PackedScene StoreScene => storeScene;
    public PackedScene MainScene => mainScene;
}