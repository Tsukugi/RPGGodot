using System.Collections.Generic;
using Godot;

public partial class RTSUI : Node {
    RealTimeStrategyPlayer player;
    readonly List<AbilityBtn> abilityButtons = new();

    public List<AbilityBtn> AbilityButtons => abilityButtons;

    public override void _Ready() {
        base._Ready();
        player = this.TryFindParentNodeOfType<RealTimeStrategyPlayer>();
        abilityButtons.Add(player.GetNode<AbilityBtn>(StaticNodePaths.PlayerUIAbilityBtn));
        abilityButtons.Add(player.GetNode<AbilityBtn>(StaticNodePaths.PlayerUIAbilityBtn2));
        abilityButtons.Add(player.GetNode<AbilityBtn>(StaticNodePaths.PlayerUIAbilityBtn3));
        abilityButtons.Add(player.GetNode<AbilityBtn>(StaticNodePaths.PlayerUIAbilityBtn4));
    }

    public override void _Input(InputEvent @event) {
        base._Input(@event);

    }
}