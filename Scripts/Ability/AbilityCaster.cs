
using Godot;

public class AbilityCaster {
    Unit casterUnit;
    AbilityDTO ability;
    public AbilityCaster(Unit casterUnit, AbilityDTO ability) {
        this.casterUnit = casterUnit;
        this.ability = ability;
    }

    public AbilityDTO Ability { get => ability; }
    public Unit CasterUnit { get => casterUnit; }

    public void Cast(AbilityCastContext context) {
        CastedAbility castedAbility = new(ability, context);
        casterUnit.AddChild(castedAbility);
        castedAbility.Cast();

        void onTargetReached() => OnAllTasksCompleted(castedAbility);
        castedAbility.OnAllTasksCompleted -= onTargetReached;
        castedAbility.OnAllTasksCompleted += onTargetReached;

    }

    void OnAllTasksCompleted(CastedAbility castedAbility) {
        GD.Print("[OnAllTasksCompleted] " + castedAbility.Name);
        castedAbility.QueueFree();
    }
}