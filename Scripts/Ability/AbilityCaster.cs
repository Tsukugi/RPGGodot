
using Godot;

public class AbilityCaster {
    Unit casterUnit;
    AbilityDTO ability;
    public AbilityCaster(Unit casterUnit, AbilityDTO ability) {
        this.casterUnit = casterUnit;
        this.ability = ability;
    }

    public AbilityDTO Ability { get => ability; }

    public void Cast(AbilityCastContext context) {
        CastedAbility castedAbility = new(ability, context);
        casterUnit.AddChild(castedAbility);
        castedAbility.Cast();
        castedAbility.OnAllTasksCompleted += () => {
            GD.Print("[OnAllTasksCompleted] " + castedAbility.Name);
            castedAbility.QueueFree();
        };
    }
}