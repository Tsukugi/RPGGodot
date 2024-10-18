using Godot;
using System.Collections.Generic;

namespace TurnBattle {
    public partial class BUIStatus : Node {

        readonly PackedScene rowScene = GD.Load<PackedScene>(ScenePaths.BUIUnitRow);
        const string rowPath = "./Flex";

        public void SetUnits(List<UnitDTO> unitDefs) {
            // Clear them
            foreach (var item in GetNode("Flex").GetChildren()) {
                item.QueueFree();
            }

            // Register new ones
            unitDefs.ForEach(unit => {
                BUIUnitRow row = rowScene.Instantiate<BUIUnitRow>();
                GetNode(rowPath).AddChild(row);
                row.Unit = unit;
            });
        }
    }
}