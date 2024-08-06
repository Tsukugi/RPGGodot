
using Godot;

public class UnitPlayerBind {
    readonly Node unit;
    PlayerBase player = null;

    public PlayerBase Player {
        get {
            player ??= GetOwner();
            return player;
        }
    }

    public UnitPlayerBind(Node unit) {
        this.unit = unit;
    }

    public PlayerBase GetOwner() {
        return unit.TryFindParentNodeOfType<PlayerBase>();
    }
}
