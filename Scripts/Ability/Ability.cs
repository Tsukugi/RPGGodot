using System;
using System.Collections.Generic;

public partial class Ability : TaskHandler {
    List<string> effectType;
    AbilityAttributesDTO attributes;
    public AbilityAttributesDTO Attributes { get => attributes; }

    public void UpdateValues(AbilityDTO ability) {
        Name = ability.name;
        effectType = ability.effectType;
        attributes = ability.attributes;
    }

    public override void Start() {
        base.Start();
        MapEffectsToQueue();
        OnTaskProcess(); // We start the timer but we want to evaluate inmediately
    }

    void MapEffectsToQueue() {
        ClearAll();
        foreach (string className in effectType) {
            Type type = Type.GetType(GetEffectName(className));
            TaskBase newEffect = (TaskBase)Activator.CreateInstance(type);
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