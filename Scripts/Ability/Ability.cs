using System;
using System.Collections.Generic;

public partial class Ability : TaskHandler {
    Unit unit;
    List<string> effects;
    AbilityAttributesDTO attributes;
    public AbilityAttributesDTO Attributes { get => attributes; }


    public Ability(AbilityDTO ability) {
        Name = ability.name;
        effects = ability.effects;
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

    public void Cast(Unit target) {
        ClearAll();
        AddEffectsToTaskQueue(target);
        StartAbility();
    }

    void AddEffectsToTaskQueue(Unit target) {
        foreach (string effectId in effects) {
            EffectBaseDTO effectBase = LocalDatabase.GetEffect(effectId);
            Type type = Type.GetType(GetEffectName(effectBase.baseEffect));
            dynamic newEffect = Activator.CreateInstance(type);
            newEffect.UpdateEffectValues(unit, this, target, effectBase);
            AddTask(newEffect);
        }
    }
    static string GetEffectName(string dtoName) {
        return "Effect" + dtoName;
    }
};