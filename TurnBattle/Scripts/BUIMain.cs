using System.Collections.Generic;
using Godot;

namespace TurnBattle {
    public partial class BUIMain : Node {
        BattleMain battle;
        BUIStatus playerStatus;
        BUIStatus enemyStatus;
        public override void _Ready() {
            base._Ready();
            battle = GetNode<BattleMain>(NodePaths.BattleRoot);
            playerStatus = GetNode<BUIStatus>(UIPaths.PlayerStatus);
            enemyStatus = GetNode<BUIStatus>(UIPaths.EnemyStatus);
        }

        public void StartStatusPanel(List<UnitDTO> unitDefs, List<UnitDTO> enemyUnitDefs) {
            playerStatus.SetUnits(unitDefs);
            enemyStatus.SetUnits(enemyUnitDefs);
        }
    }
}