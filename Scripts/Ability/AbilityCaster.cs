
using Godot;

public class AbilityCaster {
    AbilityDTO abilityAttributes;
    public AbilityCaster(AbilityDTO abilityAttributes) {
        this.abilityAttributes = abilityAttributes;
    }

    public AbilityDTO AbilityAttributes { get => abilityAttributes; }

    public void Cast(Unit caster, Unit target) {
        Ability ability = new(abilityAttributes);
        caster.AddChild(ability);
        ability.Cast(target);
        ability.OnAllTasksCompleted += () => {
            GD.Print("[OnAllTasksCompleted] " + ability.Name);
            ability.QueueFree();
        };
    }
}