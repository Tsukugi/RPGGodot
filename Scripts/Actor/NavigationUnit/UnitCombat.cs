
using System.Timers;
using Godot;
public partial class UnitCombat : Node3D {
    NavigationUnit unit = null;
    Unit target = null;
    bool isAlreadyAttacking = false;
    bool isAttackManuallyStopped = false;
    RayCast3D combatRayCast;
    public Unit Target { get => target; set => target = value; }


    public override void _Ready() {
        base._Ready();
        unit = this.TryFindParentNodeOfType<NavigationUnit>();
        combatRayCast = unit.GetNodeOrNull<RayCast3D>(StaticNodePaths.CombatRayCast);
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (target == null) {
            combatRayCast.TargetPosition = Vector3.Zero;
            return;
        }
        if (target.Attributes.CanBeKilled) {
            target = null;
            combatRayCast.TargetPosition = Vector3.Zero;
            return;
        }

        combatRayCast.TargetPosition = target.GlobalPosition - unit.GlobalPosition;
        if (combatRayCast.IsColliding()) {
            GodotObject collisionTarget = combatRayCast.GetCollider();
            if (collisionTarget is not NavigationUnit targetUnit) return;
            unit.Player.DebugLog(unit.Name + " -> " + targetUnit.Name);
            OnTargetReached(targetUnit);
        }
    }

    public void TryStartAttack() {
        if (isAlreadyAttacking || target is null) return;
        isAlreadyAttacking = true;
        if (isAttackManuallyStopped) {
            isAttackManuallyStopped = false;
            return;
        }
        CastAttack();
        TimerUtils.CreateSimpleTimer(OnAttackCastEnd, 1 / unit.Attributes.AttackCastDuration);
    }

    public void StopAttack() {
        // We only stop ongoing attack state
        if (!isAlreadyAttacking) return;
        unit.Player.DebugLog("[StopAttack] Stop attack state");
        isAttackManuallyStopped = true;
        isAlreadyAttacking = false;
    }

    void CastAttack() {
        combatRayCast.CollideWithBodies = true;
    }

    void OnTargetReached(NavigationUnit targetUnit) {
        combatRayCast.CollideWithBodies = false;
        int finalDamage = targetUnit.Attributes.ApplyDamage(unit.Attributes.BaseDamage);
        unit.Player.DebugLog("[TryStartAttack] " + unit.Name + " dealt " + finalDamage + " of damage. " +
             " \n Target hitpoints: " + targetUnit.Attributes.HitPoints);
    }


    void OnAttackCastEnd(object source, ElapsedEventArgs e) {
        TimerUtils.CreateSimpleTimer(OnAttackCooldownEnd, 1 / unit.Attributes.AttackSpeed);
        unit.Player.DebugLog("[OnAttackCastEnd]");
    }
    void OnAttackCooldownEnd(object source, ElapsedEventArgs e) {
        isAlreadyAttacking = false;
        unit.Player.DebugLog("[OnAttackCooldownEnd]");

        if (isAttackManuallyStopped) isAttackManuallyStopped = false;
        else CallDeferred("TryStartAttack");
    }
}