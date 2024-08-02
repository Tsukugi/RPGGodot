
using Godot;

public class AbilityCaster {
    Unit casterUnit;
    AbilityDTO abilityAttributes;
    public AbilityCaster(Unit casterUnit, AbilityDTO abilityAttributes) {
        this.casterUnit = casterUnit;
        this.abilityAttributes = abilityAttributes;
    }

    public AbilityDTO AbilityAttributes { get => abilityAttributes; }

    public void Cast() {
        Ability ability = new(abilityAttributes);
        casterUnit.AddChild(ability);
        ability.Cast();
        ability.OnAllTasksCompleted += () => {
            GD.Print("[OnAllTasksCompleted] " + ability.Name);
            ability.QueueFree();
        };
    }
}