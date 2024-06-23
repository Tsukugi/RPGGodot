using System.Timers;
using Godot;
public partial class UnitCombatArea : Area3D {
    NavigationUnit unit = null;
    Unit target = null;
    bool canAttack = true;
    public Unit Target { get => target; set => target = value; }

    public override void _Ready() {
        base._Ready();
        Unit parentUnit = this.TryFindUnit();
        if (parentUnit is NavigationUnit navigationUnit) {
            unit = navigationUnit;

        } else {
            GD.Print("[UnitAlertArea._Ready] " + parentUnit.Name + " has no NavigationUnit as parent, removing this Area as it is not needed.");
            QueueFree();
        }
    }

    public void TryStartAttack() {
        if (!canAttack) return;
        canAttack = false;
        target.Attributes.ApplyDamage(unit.Attributes.BaseDamage);
        TimerUtils.CreateSimpleTimer(OnAttackCooldownEnd, 1 / unit.Attributes.AttackSpeed);
    }

    void OnAttackCooldownEnd(object source, ElapsedEventArgs e) {
        canAttack = true;
        unit.Player.DebugLog("[OnAttackCooldownEnd]");
    }
}