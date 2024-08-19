using Godot;

public static class SceneUtils {
    public static void GoToScene(this Node node, PackedScene scene) {
        node.GetTree().ChangeSceneToPacked(scene);
    }

    public static void QuitGame(this Node node) {
        node.GetTree().Quit();
    }
}