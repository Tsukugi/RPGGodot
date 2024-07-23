using Godot;

public partial class EffectDamageAreaOfEffect : EffectBase {
    PackedScene areaOfEffectTemplate = GD.Load<PackedScene>(ResourcePaths.AreaOfEffect);
    AreaOfEffectUnit areaOfEffectUnit;

    public override void StartTask() {
        unit.Player.DebugLog("[EffectDamageOnCollide.StartTask] Apply damage at " + target.GlobalPosition, true);
        areaOfEffectUnit = NewEffectActor<AreaOfEffectUnit>(areaOfEffectTemplate, unit.Player, target.GlobalPosition.WithY(0.5f));
        areaOfEffectUnit.OnCollideEvent += (collider) => {
            unit.Player.DebugLog("[EffectDamageOnCollide.OnCollideEvent] Apply damage to" + collider.Name + " at " + collider.GlobalPosition, true);
        };
        base.StartTask();
        OnTaskCompleted();
    }

}
