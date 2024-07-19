using Godot;

public partial class EffectDamageOnCollide : TaskBase {
    readonly Ability ability;
    Vector3 targetPosition;

    public EffectDamageOnCollide(Vector3 targetPosition, NavigationUnit unit, Ability ability) {
        type = TaskType.Ability;
        this.targetPosition = targetPosition;
        this.unit = unit;
        this.ability = ability;
       
        unit.Player.DebugLog("[EffectDamageOnCollide] Apply damage at " + targetPosition, true);
    }
}
