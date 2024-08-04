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
    public string castType { get; set; } // We use this to define if the Player behaviour depending if its targeted or positioned -> Type is AbilityCastTypes - Required (No default)
    public double castDuration { get; set; }
    public int range { get; set; }
    public int cooldown { get; set; }
}

public class EffectBaseDTO {
    public string id { get; set; } // Id for the ability + effect
    public string baseEffect { get; set; } // The base Effect class that has the effect logic
    public string playerTypeAffected { get; set; } // We use this to differentiate behaviours to specific players -> Type is EffectPlayerTypes - default: "All"

    // * Effect specific values, an effect may use some of them but it depends on each implementation
    public int damageAmount { get; set; } // How much damage we are supposed to do
    public int healAmount { get; set; } // How much healing we are supposed to do

    // * EffectUnit values
    public int numberOfInstances { get; set; } // How much copies of a effectUnit we may instance 
    public float range { get; set; } // The range that a effectUnit may use to limit its behaviour
    public float radius { get; set; } // The radius that a effectUnit may use to define the area of effect
    public float velocity { get; set; } // The velocity that a effectUnit may use to move
}