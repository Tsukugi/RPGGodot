
using Godot;

public class EnvironmentBase {
    public readonly Plane plane = new(0, -1, 0, 0);
    public Vector3 Get3DWorldPosition(CameraBase camera, Vector2 coordinates) {
        Vector3 origin = camera.ProjectRayOrigin(coordinates);
        Vector3 direction = camera.ProjectRayNormal(coordinates);
        Vector3? position = plane.IntersectsRay(origin, direction);
        if (position is null) {
            GD.PrintErr("[Get3DWorldPosition] Could not find position for " + coordinates);
            return Vector3.Zero;
        } else {
            return (Vector3)position;
        }
    }

}