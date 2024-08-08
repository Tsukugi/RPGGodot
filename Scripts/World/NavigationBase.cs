using Godot;

public class NavigationBase : EnvironmentBase {
    public Vector3 GetNavigationTargetPosition(Camera3D camera) {
        Vector2 mousePosition = camera.GetViewport().GetMousePosition();
        Vector3 position = Get3DWorldPosition(camera, mousePosition);
        return position;
    }
}