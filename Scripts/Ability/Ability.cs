using System;
using System.Collections.Generic;

public partial class Ability : TaskHandler {
    ActorBase unit;
    List<string> effects;
    AbilityAttributesDTO attributes;
    public AbilityAttributesDTO Attributes { get => attributes; }

    ActorBase target;

    public Ability(AbilityDTO ability) {
        Name = ability.name;
        effects = ability.effects;
        attributes = ability.attributes;
    }

    public override void _Ready() {
        base._Ready();
        unit = this.TryFindParentNodeOfType<ActorBase>();
    }

    void StartAbility() {
        StartTimer();
        OnTaskProcess(); // We start the timer but we want to evaluate inmediately
    }

    public void Cast(ActorBase target) {
        this.target = target;
        MapEffectsToQueue();
        StartAbility();
    }


    void MapEffectsToQueue() {
        ClearAll();
        foreach (string effectId in effects) {
            EffectBaseDTO effectBase = PlayerManager.localDatabase.Effects[effectId];
            Type type = Type.GetType(GetEffectName(effectBase.baseEffect));
            EffectBase newEffect = (EffectBase)Activator.CreateInstance(type);

            newEffect.UpdateEffectValues(unit, this, target, effectBase);
            AddTask(newEffect);
        }
    }
    static string GetEffectName(string dtoName) {
        return "Effect" + dtoName;
    }
}

public enum AbilityType {
    UnitTargeted, AreaOfEffect, PositionTargeted, Aura,
}

public enum EffectTypeOnTarget {
    Heal, Damage, StatusEffect
}