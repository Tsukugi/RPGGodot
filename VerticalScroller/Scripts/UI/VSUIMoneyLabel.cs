
using Godot;

public partial class VSUIMoneyLabel : Label {
    protected VSStore store;
    public override void _Ready() {
        base._Ready();
        store = GetNode<VSStore>(VSNodes.Store);
    }
    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        Text = "Money: " + store.PlayerHandler.Money;
    }
}