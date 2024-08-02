using Godot;

public class AbilityCastContext {
    AbilityCaster abilityCaster;
    string type;
    Unit? target;
    Vector3? targetPosition;

    public AbilityCastContext(AbilityCaster abilityCaster) {
        this.abilityCaster = abilityCaster;
        type = this.abilityCaster.AbilityAttributes.attributes.castType;
    }

    public void AddTarget(Unit target) {
        this.target = target;
    }

    public void AddTargetPosition(Vector3 targetPosition) {
        this.targetPosition = targetPosition;
    }

    public string Type { get => type; }
    public Unit Target { get => target; }
    public Vector3? TargetPosition { get => targetPosition; }
    public AbilityCaster AbilityCaster { get => abilityCaster; }
}