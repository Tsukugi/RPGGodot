using System.Collections.Generic;
public class UnitMutableAttributes : UnitAttributes {
    readonly List<UnitAttributeMutationDTO> mutations = new();
    public List<UnitAttributeMutationDTO> Mutations => mutations;

    Dictionary<string, dynamic> appliedMutations = new();

    public UnitMutableAttributes(Unit unit) : base(unit) { }

    public AttributesExport GetMutations() {
        foreach (var appliedMutation in appliedMutations) {
            appliedMutations[appliedMutation.Key] = default;
            foreach (var mutation in mutations) {
                appliedMutations[appliedMutation.Key] = appliedMutations[appliedMutation.Key] + ApplyMutation(mutation, appliedMutations[appliedMutation.Key]);
            }
        }
        return new();
    }

    static double ApplyMutation(UnitAttributeMutationDTO mutation, double value) {
        return mutation.mutationType switch {
            MutationTypes.AddFixed => mutation.value,
            MutationTypes.AddPercentage => value * (100 / mutation.value),
            _ => value,
        };
    }

    public void AddMutation(UnitAttributeMutationDTO mutation) {
        mutations.Add(mutation);
    }
}

public static class MutationTypes {
    public const string Custom = "Custom"; // This one is really important as it defines that it must be implemented by the mutation id
    public const string AddPercentage = "AddPercentage";
    public const string AddFixed = "AddFixed";
}