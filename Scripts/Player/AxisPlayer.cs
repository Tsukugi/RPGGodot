

public partial class AxisPlayer : PlayerBase {
    readonly AxisInputHandler axisInputHandler = new();

    public AxisInputHandler AxisInputHandler { get => axisInputHandler; }
}