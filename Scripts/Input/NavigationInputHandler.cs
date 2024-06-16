

using Godot;
public partial class NavigationInputHandler : InputBase {
    public Vector3 worldMouseNavigationTargetCoordinates = Vector3.Zero;


    public static Vector2 GetAxis() {
        Vector2 axis = Vector2.Zero;

        if (Input.IsActionPressed("ui_down")) { axis.Y += 1; }
        if (Input.IsActionPressed("ui_up")) { axis.Y -= 1; }
        if (Input.IsActionPressed("ui_right")) { axis.X += 1; }
        if (Input.IsActionPressed("ui_left")) { axis.X -= 1; }

        return axis;
    }

}
