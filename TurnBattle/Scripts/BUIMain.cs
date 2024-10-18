using System.Collections.Generic;
using System.Linq;
using Godot;

namespace TurnBattle {
    public partial class BUIMain : Node {
        BattleMain battle;
        BUIStatus playerStatus;
        public override void _Ready() {
            base._Ready();
            battle = GetNode<BattleMain>(NodePaths.BattleRoot);
            playerStatus = GetNode<BUIStatus>(UIPaths.Status);
        }

        public void StartStatusPanel(List<UnitDTO> unitDefs) {
            GD.Print(unitDefs.Count);
            playerStatus.SetUnits(unitDefs);
        }
    }
}