using Godot;

public partial class EffectDamageOnCollide : EffectBase {
    new DamageAreaOfEffectDTO attributes;
    public override void StartTask() {
        type = TaskType.Effect;
        unit.Player.DebugLog("[EffectDamageOnCollide] Apply damage at " + target.GlobalPosition, true);
    }
}
