public class Ability {
    float castTime = 0;
    string name;

    AbilityType abilityType;
    EffectTypeOnTarget effectTypeOnTarget;
}

public enum AbilityType {
    UnitTargeted, AreaOfEffect, PositionTargeted, Aura,
}

public enum EffectTypeOnTarget {
    Heal, Damage, StatusEffect
}