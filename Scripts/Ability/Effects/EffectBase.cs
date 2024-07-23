
using Godot;
public class EffectBase : TaskBase {
    protected new ActorBase unit;
    protected Ability ability;
    protected EffectBaseDTO attributes;
    protected ActorBase target;
    public override void StartTask() {
        type = TaskType.Effect;
        base.StartTask();
    }

    protected T NewEffectActor<T>(PackedScene template, Node owner, Vector3 position) where T : EffectActor {
        T instance = template.Instantiate<T>();
        owner.AddChild(instance);
        instance.GlobalPosition = position;
        instance.UpdateValues(target, attributes);
        return instance;
    }

    public void UpdateEffectValues(
        ActorBase unit,
        Ability ability,
        ActorBase target,
        EffectBaseDTO attributes) {

        this.unit = unit;
        this.target = target;
        this.ability = ability;
        this.attributes = attributes;
    }
}

