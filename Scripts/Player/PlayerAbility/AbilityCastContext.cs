using System.Collections.Generic;
using Godot;

public class AbilityCastContext {
    string type;
    Unit casterUnit;
    Unit? target;
    Vector3? targetPosition;
    public AbilityCastContext(string type, Unit casterUnit) {
        this.type = type;
        this.casterUnit = casterUnit;
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
    public Unit CasterUnit { get => casterUnit; }
}