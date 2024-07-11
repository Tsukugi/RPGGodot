using System.Collections.Generic;

public class Ability {
    string name { get; set; }
    List<AbilityType> type { get; set; }
    EffectTypeOnTarget effectType { get; set; }
    float velocity { get; set; }
    AbilityAttributesDTO attributes { get; set; }
}

public enum AbilityType {
    UnitTargeted, AreaOfEffect, PositionTargeted, Aura,
}

public enum EffectTypeOnTarget {
    Heal, Damage, StatusEffect
}