using System.Collections.Generic;
using Godot;

public partial class VSUIBase : Node {
    protected VSStore store;
    protected readonly Dictionary<string, VSUIButton> mainMenuButtons = new();

    public override void _Ready() {
        base._Ready();
        store = GetNode<VSStore>(VSNodes.Store);
    }

    protected void AttachEvents(List<string> btnNames, string path) {
        btnNames.ForEach(action => {
            GD.Print(action);
            VSUIButton btn = GetNode<VSUIButton>(path + action);
            mainMenuButtons.Add(action, btn);

            if (btn.Action is VSUIButtonAction.None) return;

            btn.Pressed += () => OnAction(btn.Action);
        });
    }

    public void OnAction(VSUIButtonAction action) {
        switch (action) {
            case VSUIButtonAction.BackToMain: GetTree().ChangeSceneToPacked(store.MainScene); return;
            case VSUIButtonAction.StartGame: GetTree().ChangeSceneToPacked(store.GameScene); return;
            case VSUIButtonAction.GoToStore: GetTree().ChangeSceneToPacked(store.StoreScene); return;
            case VSUIButtonAction.Exit: GetTree().Quit(); return;
        }
    }
}