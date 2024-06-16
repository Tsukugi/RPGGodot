
public partial class InputBase {
    protected InputFaceDirection inputFaceDirection = InputFaceDirection.Down;
    public InputFaceDirection InputFaceDirection { get => inputFaceDirection; }

}

public enum InputState {
    Stop,
    Move,
    Cast,
    Attack,
}

public enum ActionState {
    Idle,
    Attack,
    Cast,
}

public enum InputFaceDirection {
    Down,
    Up,
    Left,
    Right
}
