
using Godot;

public partial class EffectHeal : EffectBase {
    PackedScene areaOfEffectTemplate = GD.Load<PackedScene>(ResourcePaths.AreaOfEffect);
    AreaOfEffectUnit areaOfEffectUnit;

    public override void StartTask() {
        if (abilityCastContext.Target is not Unit target) return;
        unit.Player.DebugLog("[EffectHeal] Apply damage at " + target.GlobalPosition);
        areaOfEffectUnit = NewEffectActor<AreaOfEffectUnit>(areaOfEffectTemplate, unit.Player, target.GlobalPosition.AddToY(0.5f));
        areaOfEffectUnit.OnCollideEvent += (collider) => {
            unit.Player.DebugLog("[EffectHeal] Healing to" + collider.Name + " at " + collider.GlobalPosition, true);
            collider.Attributes.ApplyHeal(attributes.healAmount);
        };
        base.StartTask();
        OnTaskCompleted();
    }

}
