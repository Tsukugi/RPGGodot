using System;
using System.Collections.Generic;
using Godot;

public partial class CastedAbility : TaskHandler {
    Unit unit;
    readonly List<string> effectIds;
    readonly AbilityAttributesDTO attributes;
    readonly AbilityCastContext context;

    new protected Timer taskProcessTimer = new() {
        OneShot = false,
        WaitTime = 0.2f,
    };

    public AbilityAttributesDTO Attributes { get => attributes; }
    public AbilityCastContext Context { get => context; }

    public CastedAbility(AbilityDTO ability, AbilityCastContext context) {
        Name = ability.name;
        effectIds = ability.effects;
        attributes = ability.attributes;
        this.context = context;
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
            EffectBaseDTO effectBase = unit.Player.PlayerManager.Database.GetEffect(effectId);
            Type type = Type.GetType(GetEffectName(effectBase.baseEffect));
            dynamic newEffect = Activator.CreateInstance(type);
            newEffect.UpdateEffectValues(unit, this, context, effectBase);
            AddTask(newEffect);
        }
    }
    static string GetEffectName(string dtoName) {
        return "Effect" + dtoName;
    }
};