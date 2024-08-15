using Godot;


public partial class AttackCollisionArea : Area3D {

    AxisPlayer player;
    CollisionShape3D meleeCollisionArea;

    public CollisionShape3D MeleeCollisionArea { get => meleeCollisionArea; }

    public override void _Ready() {
        player = this.TryFindParentNodeOfType<AxisPlayer>();
        meleeCollisionArea = GetNode<CollisionShape3D>(AxisNodeNames.MeleeCollisionArea);

        AreaEntered -= OnMeleeAttackAreaEnteredHandler;
        AreaEntered += OnMeleeAttackAreaEnteredHandler;
        AreaExited -= OnMeleeAttackAreaExitedHandler;
        AreaExited += OnMeleeAttackAreaExitedHandler;
    }

    void OnMeleeAttackAreaEnteredHandler(Area3D area) {
        if (area.Name != AxisNodeNames.AttackArea) return;
        if (!player.IsFirstPlayer()) return;
        Unit attackedUnit = (Unit)area.GetParent().GetParent();
        AttributesExport attributes = attackedUnit.GetAttributes();
        GD.Print("[OnMeleeAttackAreaEnteredHandler] " + attackedUnit.Name);
        GD.Print("[OnMeleeAttackAreaEnteredHandler] Applying " + attributes.BaseDamage + " of base damage to " + attackedUnit.Name);
        attackedUnit.UnitAttributes.ApplyDamage(attributes.BaseDamage);
        GD.Print("[OnMeleeAttackAreaEnteredHandler] HP: " + attributes.HitPoints + " / " + attributes.MaxHitPoints);

        if (attackedUnit.UnitAttributes.CanBeKilled) {
            attackedUnit.Visible = false;
        }
    }

    void OnMeleeAttackAreaExitedHandler(Area3D area) {
        if (area.Name != AxisNodeNames.AttackArea) return;
        if (!player.IsFirstPlayer()) return;
        Unit attackedUnit = (Unit)area.GetParent().GetParent();
        GD.Print("[OnMeleeAttackAreaExitedHandler] " + attackedUnit.Name);
    }
}
