using System.Collections.Generic;
public class UnitMutableAttributes : UnitAttributes {
    readonly UnitWSBind unitWSBind;
    readonly List<MutationDTO> mutations = new();
    UnitAttributesDTO mutatedAttributes = new();
    UnitAttributesDTO appliedMutations = new();
    public List<MutationDTO> Mutations => mutations;
    public UnitMutableAttributes(Unit unit, UnitWSBind unitWSBind) : base(unit) {
        this.unitWSBind = unitWSBind;
    }

    public void ApplyMutations(UnitAttributesDTO appliedMutations) {
        this.appliedMutations = appliedMutations;
    }

    public void ApplyMutatedAttributes(UnitAttributesDTO mutatedAttributes) {
        this.mutatedAttributes = mutatedAttributes;
    }

    public UnitAttributesDTO GetMutations() {
        return appliedMutations;
    }
    public UnitAttributesDTO GetMutatedAttributes() {
        return mutatedAttributes;
    }

    public void AddMutation(MutationDTO mutation) {
        mutations.Add(mutation);
        unitWSBind.AddMutation(mutation);
        unitWSBind.GetMutatedAttributes();
    }
}

public static class MutationTypes {
    public const string Custom = "Custom"; // This one is really important as it defines that it must be implemented by the mutation id
    public const string AddPercentage = "AddPercentage";
    public const string AddFixed = "AddFixed";
}
public static class MutationTargets {
    public const string Unit = "Unit";
    public const string Player = "Player";
}