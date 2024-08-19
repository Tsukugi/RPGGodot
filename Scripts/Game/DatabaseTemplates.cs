using System.Collections.Generic;



public class DTOBase {
    public string id;
}

public class UnitDTO : DTOBase {
    public string name;
    public UnitAttributesDTO attributes;
    public string[] abilities;
}

public class UnitAttributesDTO : DTOBase {
    public float movementSpeed;
    public int maxHitPoints;
    public double attackSpeed;
    public int armor;
    public int baseDamage;
    public double attackCastDuration;
    public float attackRange;
    public int hitPoints;
    public float alertRange;
}

public class AbilityDTO : DTOBase {
    public string name;
    public List<string> effects;
    public AbilityAttributesDTO attributes;
}

public class AbilityAttributesDTO : DTOBase {
    public string castType; // We use this to define if the Player behaviour depending if its targeted or positioned -> Type is AbilityCastTypes - Required (No default)
    public double castDuration;
    public int range;
    public int cooldown;
}

public class EffectBaseDTO : DTOBase {
    // * It is recomended for the Id to be ability + effect
    public string baseEffect; // The base Effect class that has the effect logic
    public string playerTypeAffected; // We use this to differentiate behaviours to specific players -> Type is EffectPlayerTypes - default: "All"

    // * Effect specific values, an effect may use some of them but it depends on each implementation
    public int damageAmount; // How much damage we are supposed to do
    public int healAmount; // How much healing we are supposed to do

    // * EffectUnit values
    public int numberOfInstances; // How much copies of a effectUnit we may instance 
    public float range; // The range that a effectUnit may use to limit its behaviour
    public float radius; // The radius that a effectUnit may use to define the area of effect
    public float velocity; // The velocity that a effectUnit may use to move
}

public class UnitAttributeMutationDTO : DTOBase {
    public string attributeName;
    public string mutationType; // Typed as MutationTypes
    public float value;
}