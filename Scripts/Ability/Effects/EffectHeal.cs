using Godot;

public partial class EffectHeal : EffectBase {
    readonly PackedScene areaOfEffectTemplate = GD.Load<PackedScene>(ResourcePaths.AreaOfEffect);
    AreaOfEffectUnit areaOfEffectUnit;
    public override void StartTask() {
        if (abilityCastContext.Target is not Unit target) return;
        unit.Player.DebugLog("[EffectHeal] Healing " + target.Name, true);
        areaOfEffectUnit = NewEffectActor<AreaOfEffectUnit>(areaOfEffectTemplate, unit.Player, target.GlobalPosition.AddToY(0.5f));

        void CollideEvent(Unit collider) {
            unit.Player.DebugLog("[EffectHeal] Healing to " + collider.Name + " at " + collider.GlobalPosition, true);
            collider.Attributes.ApplyHeal(attributes.healAmount);
        }
        areaOfEffectUnit.OnCollideEvent -= CollideEvent;
        areaOfEffectUnit.OnCollideEvent += CollideEvent;
        base.StartTask();
        OnTaskCompleted();
    }

}
