using Godot;

namespace TurnBattle {
    public partial class BUIUnitRow : Node {

        UnitDTO unit;
        RichTextLabel name;
        RichTextLabel attackTimer;

        public UnitDTO Unit {
            get => unit; set {
                unit = value;
                name.Text = unit.name;
                var attributes = unit.attributes;
                attackTimer.Text = $"{attributes.hitPoints}/{attributes.maxHitPoints}";
            }
        }

        public override void _Ready() {
            name = GetNode<RichTextLabel>("./Name");
            attackTimer = GetNode<RichTextLabel>("./AttackTimer");
        }

        public override void _Process(double delta) {
        }
    }
}