using System.Collections.Generic;
using Godot;

public partial class VSUIGame : VSUIBase {

    const string btnPath = "";
    readonly List<string> btnNames = new() { "BackBtn" };

    public override void _Ready() {
        base._Ready();
        AttachEvents(btnNames, btnPath);
    }
}