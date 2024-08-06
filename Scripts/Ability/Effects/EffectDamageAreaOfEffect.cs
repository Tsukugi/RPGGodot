using Godot;

public partial class EffectDamageAreaOfEffect : EffectBase {
    PackedScene areaOfEffectTemplate = GD.Load<PackedScene>(ResourcePaths.AreaOfEffect);
    AreaOfEffectUnit areaOfEffectUnit;

    public override void StartTask() {
        if (abilityCastContext.TargetPosition is not Vector3 targetPosition) return;
        unit.Player.DebugLog("[EffectDamageAreaOfEffect] " + targetPosition);
        areaOfEffectUnit = NewEffectActor<AreaOfEffectUnit>(areaOfEffectTemplate, unit.Player, targetPosition.AddToY(0.5f));

        void OnCollide(Unit collider) {
            unit.Player.DebugLog("[EffectDamageAreaOfEffect] Apply damage to" + collider.Name + " at " + collider.GlobalPosition, true);
            collider.Attributes.ApplyDamage(attributes.damageAmount);
        }
        areaOfEffectUnit.OnCollideEvent -= OnCollide;
        areaOfEffectUnit.OnCollideEvent += OnCollide;
        base.StartTask();
        OnTaskCompleted();
    }
}
