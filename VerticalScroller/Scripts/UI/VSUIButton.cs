using Godot;

public partial class VSUIButton : Button {
    [Export]
    VSUIButtonAction action = VSUIButtonAction.None;

    public VSUIButtonAction Action { get => action; }

}


public enum VSUIButtonAction {
    None,
    StartGame,
    GoToStore,
    Exit,
    BackToMain,
}