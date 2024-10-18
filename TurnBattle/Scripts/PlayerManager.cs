using Godot;


namespace TurnBattle {
    public partial class PlayerManager : Node {
        BattleMain battle;
        public override void _Ready() {
            base._Ready();
            battle = GetNode<BattleMain>(NodePaths.BattleRoot);
        }
    }
}