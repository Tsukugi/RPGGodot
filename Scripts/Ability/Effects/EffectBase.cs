
public class EffectBase : TaskBase {
    protected new ActorBase unit;
    protected Ability ability;
    protected ActorBase target;

    public void UpdateEffectValues(ActorBase unit, Ability ability, ActorBase target) {
        this.unit = unit;
        this.target = target;
        this.ability = ability;
    }
}

