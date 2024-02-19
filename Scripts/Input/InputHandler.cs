
using System;
using Godot;
public enum InputState {
    Stop,
    Move,
    StartAnimation,
}

public enum InputFaceDirection {
    Down,
    Up,
    Left,
    Right
}


public partial class InputHandler {

    private InputState inputState = InputState.Stop;
    private InputFaceDirection inputFaceDirection = InputFaceDirection.Down;
    private Vector2 moveDirection = Vector2.Zero;

    public InputState GetInputState() {
        return inputState;
    }
    public InputFaceDirection GetInputFaceDirection() {
        return inputFaceDirection;
    }

    public bool GetAxisChange() {
        Vector2 axis = GetAxis();
        return moveDirection != axis;
    }

    public Vector2 GetAxis() {
        Vector2 axis = Vector2.Zero;

        if (Input.IsActionPressed("ui_right")) { axis.X += 1; inputFaceDirection = InputFaceDirection.Right; }
        if (Input.IsActionPressed("ui_left")) { axis.X -= 1; inputFaceDirection = InputFaceDirection.Left; }
        if (Input.IsActionPressed("ui_down")) { axis.Y += 1; inputFaceDirection = InputFaceDirection.Down; }
        if (Input.IsActionPressed("ui_up")) { axis.Y -= 1; inputFaceDirection = InputFaceDirection.Up; }

        return axis;
    }


    public Vector2 GetRotatedAxis(float verticalRotation) {
        return VectorUtils.Rotate(GetAxis(), verticalRotation);
    }


    public void FrameUpdate() {
        moveDirection = GetAxis();
        if (moveDirection.Length() > 0) inputState = InputState.Move;
        else inputState = InputState.Stop;
    }

}
