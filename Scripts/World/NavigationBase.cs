using Godot;

public class NavigationBase {
    public readonly Plane plane = new(0, -1, 0, 0);

    public Vector3? GetNavigationTargetPosition(CameraBase camera) {
        Vector2 mousePosition = camera.GetViewport().GetMousePosition();
        Vector3? position = Get3DWorldPosition(camera, mousePosition);
        GD.Print((Vector3)position);
        return position;
    }


    public Vector3? Get3DWorldPosition(CameraBase camera, Vector2 coordinates) {
        Vector3 origin = camera.ProjectRayOrigin(coordinates);
        Vector3 direction = camera.ProjectRayNormal(coordinates);
        Vector3? position = plane.IntersectsRay(origin, direction);
        return position;
    }
}