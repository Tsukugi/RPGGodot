using System;
using System.Collections.Generic;

public partial class Ability : TaskHandler {
    Unit unit;
    List<string> effectIds;
    AbilityAttributesDTO attributes;
    public AbilityAttributesDTO Attributes { get => attributes; }


    public Ability(AbilityDTO ability) {
        Name = ability.name;
        effectIds = ability.effects;
        attributes = ability.attributes;
    }

    public override void _Ready() {
        base._Ready();
        unit = this.TryFindParentNodeOfType<Unit>();
    }

    void StartAbility() {
        StartTimer();
        OnTaskProcess(); // We start the timer but we want to evaluate inmediately
    }

    public void Cast() {
        ClearAll();
        AddEffectsToTaskQueue();
        StartAbility();
    }

    void AddEffectsToTaskQueue() {
        foreach (string effectId in effectIds) {
            EffectBaseDTO effectBase = LocalDatabase.GetEffect(effectId);
            Type type = Type.GetType(GetEffectName(effectBase.baseEffect));
            dynamic newEffect = Activator.CreateInstance(type);
            newEffect.UpdateEffectValues(unit, this, effectBase);
            AddTask(newEffect);
        }
    }
    static string GetEffectName(string dtoName) {
        return "Effect" + dtoName;
    }
};