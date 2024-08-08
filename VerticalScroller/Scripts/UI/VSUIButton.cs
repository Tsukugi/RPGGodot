using Godot;

public partial class VSUIButton : Button {

    [Export]
    VSUIButtonAction action = VSUIButtonAction.None;

    VSUIMain main;

    public override void _Ready() {
        base._Ready();
        main = this.TryFindParentNodeOfType<VSUIMain>();
        Pressed += OnPressed;
    }

    void OnPressed() {
        if (action is VSUIButtonAction.None) return;
        switch (action) {
            case VSUIButtonAction.BackToMain: GetTree().ChangeSceneToPacked(main.MainScene); return;
            case VSUIButtonAction.StartGame: GetTree().ChangeSceneToPacked(main.GameScene); return;
            case VSUIButtonAction.GoToStore: GetTree().ChangeSceneToPacked(main.StoreScene); return;
            case VSUIButtonAction.Exit: GetTree().Quit(); return;
        }
    }
}


enum VSUIButtonAction {
    None,
    StartGame,
    GoToStore,
    Exit,
    BackToMain,
}