
public class EffectBase : TaskBase {
    protected new ActorBase unit;
    protected Ability ability;
    protected EffectBaseDTO attributes;
    protected ActorBase target;

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

