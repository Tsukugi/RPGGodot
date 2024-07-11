
using System.Timers;
using Godot;
public partial class UnitCombat : Node3D {
    NavigationUnit unit = null;
    Unit target = null;
    bool isAlreadyAttacking = false;
    bool isAttackManuallyStopped = false;
    bool isInCombat = false;
    RayCast3D combatRayCast;

    public string TargetName { get => isInCombat ? target.Name : " "; }
    public bool IsInCombat { get => isInCombat; }
    public delegate void OnCombatEvent();
    public event OnCombatEvent OnCombatEndEvent;
    public event OnCombatEvent OnAttackReachedEvent;
    public event OnCombatEvent OnAttackFailedEvent;

    public override void _Ready() {
        base._Ready();
        unit = this.TryFindParentNodeOfType<NavigationUnit>();
        combatRayCast = unit.GetNodeOrNull<RayCast3D>(StaticNodePaths.CombatRayCast);
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);

        if (target is null || target.Attributes.CanBeKilled) EndCombat();
    }


    public void StartCombatTask(Unit target) {
        StartCombat(target);
        UnitTaskAttack newAttackTask = new(target, unit);
        newAttackTask.OnTaskCompletedEvent += (TaskBase task) => EndCombat();
        unit.UnitTask.PriorityAddTask(newAttackTask);
    }

    public void StartCombat(Unit target) {
        unit.Player.DebugLog("[StartCombat] " + unit.Name + " -> " + target.Name);
        this.target = target;
        isInCombat = true;

    }

    public void EndCombat() {
        if (!isInCombat) return;
        unit.Player.DebugLog("[EndCombat] " + unit.Name + " has finished combat");
        target = null;
        isInCombat = false;
        OnCombatEndEvent?.Invoke();
    }

    public void TryStartAttack() {
        if (isAlreadyAttacking || !isInCombat) return;
        isAlreadyAttacking = true;
        if (isAttackManuallyStopped) {
            isAttackManuallyStopped = false;
            return;
        }
        CastAttack();
        if (unit.Attributes.AttackCastDuration > 0) TimerUtils.CreateSimpleTimer((s, e) => OnAttackCastEnd(), unit.Attributes.AttackCastDuration);
        else OnAttackCastEnd();
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
        combatRayCast.TargetPosition = target.GlobalPosition + Vector3.Up.Magnitude(0.5f) - unit.GlobalPosition;
        combatRayCast.ForceRaycastUpdate();
        unit.Player.DebugLog("[CastAttack] " + unit.Name + " is attacking to " + combatRayCast.TargetPosition);

        if (combatRayCast.IsColliding()) {
            GodotObject collisionTarget = combatRayCast.GetCollider();
            if (collisionTarget is NavigationUnit targetUnit) {
                unit.Player.DebugLog("[CastAttack] " + unit.Name + " -> " + targetUnit.Name, true);
                OnTargetAttackReached(targetUnit);
            } else {
                OnAttackFailedEvent?.Invoke();
            }
        } else {
            OnAttackFailedEvent?.Invoke();
        }
        combatRayCast.CollideWithBodies = false;
    }


    void OnTargetAttackReached(NavigationUnit targetUnit) {
        OnAttackReachedEvent?.Invoke();
        int finalDamage = targetUnit.Attributes.ApplyDamage(unit.Attributes.BaseDamage);
        unit.Player.DebugLog("[TryStartAttack] " + unit.Name + " dealt " + finalDamage + " of damage. " +
             " \n Target hitpoints: " + targetUnit.Attributes.HitPoints);
    }
    void OnAttackCastEnd() {
        TimerUtils.CreateSimpleTimer((s, e) => OnAttackCooldownEnd(), unit.Attributes.AttackSpeed);
        unit.Player.DebugLog("[OnAttackCastEnd]");
    }

    void OnAttackCooldownEnd() {
        isAlreadyAttacking = false;
        unit.Player.DebugLog("[OnAttackCooldownEnd]");

        if (isAttackManuallyStopped) isAttackManuallyStopped = false;
        else CallDeferred("TryStartAttack");
    }
}