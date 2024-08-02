
using Godot;

public partial class EffectHeal : EffectBase {
    PackedScene areaOfEffectTemplate = GD.Load<PackedScene>(ResourcePaths.AreaOfEffect);
    AreaOfEffectUnit areaOfEffectUnit;

    public override void StartTask() {
        unit.Player.DebugLog("[EffectDamageOnCollide.StartTask] Apply damage at " + target.GlobalPosition);
        areaOfEffectUnit = NewEffectActor<AreaOfEffectUnit>(areaOfEffectTemplate, unit.Player, target.GlobalPosition.AddToY(0.5f));
        areaOfEffectUnit.OnCollideEvent += (collider) => {
            unit.Player.DebugLog("[EffectHealTarget.OnCollideEvent] Healing to" + collider.Name + " at " + collider.GlobalPosition, true);
            collider.Attributes.ApplyHeal(attributes.healAmount);
        };
        base.StartTask();
        OnTaskCompleted();
    }

}
