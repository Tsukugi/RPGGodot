using System.Collections.Generic;
using Godot;

public class Ability : Node {
    List<string> effectType;
    AbilityAttributesDTO attributes;

    Timer processTimer = new() {
        OneShot = false,
        WaitTime = 0.5f,
    };

    public override void _Ready() {
        base._Ready();
        AddChild(processTimer);
        processTimer.Timeout += OnProcess;
        processTimer.Start();
    }

    public void Update(AbilityDTO ability) {
        Name = ability.name;
        effectType = ability.effectType;
        attributes = ability.attributes;
    }

    public void OnFinished() {
    }

    public void OnProcess() {
    }
}

public enum AbilityType {
    UnitTargeted, AreaOfEffect, PositionTargeted, Aura,
}

public enum EffectTypeOnTarget {
    Heal, Damage, StatusEffect
}