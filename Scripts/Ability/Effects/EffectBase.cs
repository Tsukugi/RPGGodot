
using Godot;
/*
 * Important name convention: Use Effect${NameOfTheEffect} as class name, 
 * as it is required for the generic Effect loader.
*/
public class EffectBase : TaskBase {
    protected new Unit unit;
    protected CastedAbility ability;
    protected EffectBaseDTO attributes;
    protected AbilityCastContext abilityCastContext;
    public override void StartTask() {
        type = TaskType.Effect;
        base.StartTask();
    }

    protected T NewEffectActor<T>(PackedScene template, Node owner, Vector3 position) where T : EffectUnit {
        T instance = template.Instantiate<T>();
        owner.AddChild(instance);
        instance.GlobalPosition = position;
        instance.InitializeEffectUnit(attributes, abilityCastContext);
        return instance;
    }

    public void UpdateEffectValues(
        Unit unit,
        CastedAbility ability,
        AbilityCastContext abilityCastContext,
        EffectBaseDTO attributes) {

        this.unit = unit;
        this.ability = ability;
        this.abilityCastContext = abilityCastContext;
        this.attributes = attributes;
    }
}

