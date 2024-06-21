

using Godot;
using System;


public partial class AxisInputHandler : InputBase {

    InputState movementInputState = InputState.Stop;
    ActionState actionInputState = ActionState.Idle;
    Vector2 moveDirection = Vector2.Zero;


    public InputState MovementInputState { get => movementInputState; }
    public ActionState ActionInputState { get => actionInputState; }


    public bool GetAxisChange() {
        Vector2 axis = GetAxis();
        return moveDirection != axis;
    }

    public Vector2 GetAxis() {
        Vector2 axis = Vector2.Zero;

        if (Input.IsActionPressed("ui_down")) { axis.Y += 1; inputFaceDirection = InputFaceDirection.Down; }
        if (Input.IsActionPressed("ui_up")) { axis.Y -= 1; inputFaceDirection = InputFaceDirection.Up; }
        if (Input.IsActionPressed("ui_right")) { axis.X += 1; inputFaceDirection = InputFaceDirection.Right; }
        if (Input.IsActionPressed("ui_left")) { axis.X -= 1; inputFaceDirection = InputFaceDirection.Left; }

        return axis;
    }

    public bool IsAttacking() {
        return Input.IsActionPressed("Attack");
    }


    public Vector2 GetRotatedAxis(float verticalRotation) {
        return VectorUtils.Rotate(GetAxis(), verticalRotation);
    }

    public void OnInputUpdate() {
        moveDirection = GetAxis();

        if (IsAttacking()) actionInputState = ActionState.Attack;
        else actionInputState = ActionState.Idle;

        if (moveDirection.Length() > 0) movementInputState = InputState.Move;
        else movementInputState = InputState.Stop;
    }

}
