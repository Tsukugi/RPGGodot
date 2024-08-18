using Godot;

public partial class VSStore : Node {

    readonly PackedScene mainScene = GD.Load<PackedScene>(VSScenePaths.Main);
    readonly PackedScene storeScene = GD.Load<PackedScene>(VSScenePaths.Store);
    readonly PackedScene gameScene = GD.Load<PackedScene>(VSScenePaths.Game);
    readonly VSPlayerHandler playerHandler = new();

    public PackedScene GameScene => gameScene;
    public PackedScene StoreScene => storeScene;
    public PackedScene MainScene => mainScene;
    public VSPlayerHandler PlayerHandler { get => playerHandler; }
}