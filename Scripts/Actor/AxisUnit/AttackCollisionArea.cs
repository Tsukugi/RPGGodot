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
        GD.Print("[OnMeleeAttackAreaEnteredHandler] " + attackedUnit.Name);
        GD.Print("[OnMeleeAttackAreaEnteredHandler] Applying " + attackedUnit.Attributes.BaseDamage + " of base damage to " + attackedUnit.Name);
        attackedUnit.Attributes.ApplyDamage(attackedUnit.Attributes.BaseDamage);
        GD.Print("[OnMeleeAttackAreaEnteredHandler] HP: " + attackedUnit.Attributes.HitPoints + " / " + attackedUnit.Attributes.MaxHitPoints);

        if (attackedUnit.Attributes.CanBeKilled) {
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
