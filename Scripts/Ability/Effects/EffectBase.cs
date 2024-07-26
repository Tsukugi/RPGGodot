
using Godot;
/*
 * Important name convention: Use Effect${NameOfTheEffect} as class name, 
 * as it is required for the generic Effect loader.
*/
public class EffectBase : TaskBase {
    protected new Unit unit;
    protected Ability ability;
    protected EffectBaseDTO attributes;
    protected Unit target;
    public override void StartTask() {
        type = TaskType.Effect;
        base.StartTask();
    }

    protected T NewEffectActor<T>(PackedScene template, Node owner, Vector3 position) where T : EffectUnit {
        T instance = template.Instantiate<T>();
        owner.AddChild(instance);
        instance.GlobalPosition = position;
        instance.UpdateValues(target, attributes);
        return instance;
    }

    public void UpdateEffectValues(
        Unit unit,
        Ability ability,
        Unit target,
        EffectBaseDTO attributes) {

        this.unit = unit;
        this.target = target;
        this.ability = ability;
        this.attributes = attributes;
    }
}

