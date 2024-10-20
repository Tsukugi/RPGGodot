using System;
using System.Collections.Generic;
using Godot;


namespace TurnBattle {
    public partial class Player : Node {
        PackedScene unitTemplate = GD.Load<PackedScene>(ScenePaths.BattleUnit);
        List<UnitDTO> units = new();

        public override void _Ready() {
            base._Ready();
            List<UnitDTO> unitDefs = new();
            foreach (var item in GetChildren()) {
                if (item is not Unit unit) continue;
                unitDefs.Add(unit.UnitInfo);
            }
            RegisterUnits(unitDefs);
        }

        public List<UnitDTO> GetUnits() {
            return units;
        }

        public void RegisterUnits(List<UnitDTO> unitDefs) {
            double magnitudeOffset = 150;

            units = new();
            foreach (var unit in GetChildren()) {
                unit.QueueFree();
            }
            foreach (UnitDTO unitDef in unitDefs) {
                Unit newUnit = unitTemplate.Instantiate<Unit>();
                AddChild(newUnit);
                newUnit.UpdateUnit(unitDef);
                newUnit.Position = new Vector2(
                    (float)((magnitudeOffset * new Random().NextDouble()) - magnitudeOffset / 2),
                    (float)((magnitudeOffset * new Random().NextDouble()) - magnitudeOffset / 2)
                );

                GD.Print(newUnit.Position);
                units.Add(newUnit.UnitInfo);
            }
        }
    }
}