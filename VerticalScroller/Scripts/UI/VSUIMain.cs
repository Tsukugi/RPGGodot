using System.Collections.Generic;
using Godot;

public partial class VSUIMain : VSUIBase {
    const string btnPath = "GridContainer/";
    readonly List<string> btnNames = new() { "StartGameBtn", "StoreBtn", "ExitBtn" };

    public override void _Ready() {
        base._Ready();
        AttachEvents(btnNames, btnPath);
    }
}