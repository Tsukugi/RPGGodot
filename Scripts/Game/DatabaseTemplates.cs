using System.Collections.Generic;

public class NavigationUnitDTO {
    public string name { get; set; }
    public NavigationUnitAttributesDTO attributes { get; set; }
    public string[] abilities { get; set; }
}

public class NavigationUnitAttributesDTO {
    public float movementSpeed { get; set; }
    public int maxHitPoints { get; set; }
    public double attackSpeed { get; set; }
    public int armor { get; set; }
    public int baseDamage { get; set; }
    public double attackCastDuration { get; set; }
    public float attackRange { get; set; }
}

public class AbilityDTO {
    public string name { get; set; }
    public List<string> effects { get; set; }
    public AbilityAttributesDTO attributes { get; set; }
}

public class AbilityAttributesDTO {
    public double castDuration { get; set; }
    public int range { get; set; }
    public int cooldown { get; set; }
}



public class EffectBaseDTO {
    public string id { get; set; }
    public string baseEffect { get; set; }
}
public class DamageAreaOfEffectDTO : EffectBaseDTO {
    public int damage { get; set; }
    public float range { get; set; }
    public float velocity { get; set; }
}

public class ProjectileDTO : EffectBaseDTO {
    public float velocity { get; set; }
}

public class HealTargetDTO : EffectBaseDTO {
    public int amount { get; set; }
}
