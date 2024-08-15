

using Godot;


public partial class AxisInputHandler : InputBase {

    InputState movementInputState = InputState.Stop;
    UnitActionState actionInputState = UnitActionState.Idle;
    Vector2 moveDirection = Vector2.Zero;


    public InputState MovementInputState { get => movementInputState; }
    public UnitActionState ActionInputState { get => actionInputState; }

    public AxisType InputAxisType = AxisType.FullAxis;


    public bool GetAxisChange() {
        Vector2 axis = GetAxis();
        return moveDirection != axis;
    }

    public Vector2 GetAxis() {
        Vector2 axis = InputAxisType switch {
            AxisType.XAxis => GetXAxis(),
            AxisType.YAxis => GetYAxis(),
            AxisType.FourAxis => GetFourAxis(),
            AxisType.FullAxis => GetFullAxis(),
            _ => Vector2.Zero,
        };
        ApplyUnitRenderDirection(axis);
        return axis;
    }

    Vector2 GetXAxis() {
        Vector2 axis = Vector2.Zero;
        if (Input.IsActionPressed("ui_right")) axis.X++;
        if (Input.IsActionPressed("ui_left")) axis.X--;
        return axis;
    }
    Vector2 GetYAxis() {
        Vector2 axis = Vector2.Zero;
        if (Input.IsActionPressed("ui_down")) axis.Y++;
        if (Input.IsActionPressed("ui_up")) axis.Y--;
        return axis;
    }
    Vector2 GetFourAxis() {
        Vector2 axis = Vector2.Zero;
        if (Input.IsActionPressed("ui_right")) return Vector2.Right;
        if (Input.IsActionPressed("ui_left")) return Vector2.Left;
        if (Input.IsActionPressed("ui_down")) return Vector2.Down;
        if (Input.IsActionPressed("ui_up")) return Vector2.Up;
        return axis;
    }
    Vector2 GetFullAxis() {
        Vector2 axis = Vector2.Zero;
        if (Input.IsActionPressed("ui_right")) axis.X++;
        if (Input.IsActionPressed("ui_left")) axis.X--;
        if (Input.IsActionPressed("ui_down")) axis.Y++;
        if (Input.IsActionPressed("ui_up")) axis.Y--;
        return axis;
    }


    void ApplyUnitRenderDirection(Vector2 axis) {
        switch (axis.X) {
            case 1: renderDirection = UnitRenderDirection.Right; break;
            case -1: renderDirection = UnitRenderDirection.Left; break;
        }
        switch (axis.Y) {
            case 1: renderDirection = UnitRenderDirection.Down; break;
            case -1: renderDirection = UnitRenderDirection.Up; break;
        }
    }


    public bool IsAttacking() {
        return Input.IsActionPressed("Attack");
    }


    public Vector2 GetRotatedAxis(float verticalRotation) {
        return VectorUtils.Rotate(GetAxis(), verticalRotation);
    }

    public void OnInputUpdate() {
        moveDirection = GetAxis();

        if (IsAttacking()) actionInputState = UnitActionState.Attack;
        else actionInputState = UnitActionState.Idle;

        if (moveDirection.Length() > 0) movementInputState = InputState.Move;
        else movementInputState = InputState.Stop;
    }

}


public enum AxisType {
    XAxis,
    YAxis,
    FullAxis,
    FourAxis,
}