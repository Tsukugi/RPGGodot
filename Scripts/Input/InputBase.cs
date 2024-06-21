
public partial class InputBase {
    protected UnitRenderDirection renderDirection = UnitRenderDirection.Down;
    public UnitRenderDirection RenderDirection { get => renderDirection; }

}

public enum InputState {
    Stop,
    Move,
    Cast,
    Attack,
}