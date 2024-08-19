using System.Collections.Generic;

public class UnitMutableAttributes : UnitAttributes {
    List<UnitAttributeMutationDTO> mutations = new();

    public UnitMutableAttributes(Unit unit) : base(unit) { }

    public List<UnitAttributeMutationDTO> Mutations { get => mutations; }

    public new AttributesExport GetAttributes() {
        return ApplyMutations(base.GetAttributes());
    }

    public AttributesExport ApplyMutations(AttributesExport attributes) {
        mutations.ForEach(mutation => {
            attributes.SetObjectProperty(mutation.attributeName, mutation.value);
        });
        return attributes;
    }

    public void AddMutation(UnitAttributeMutationDTO mutation) {
        mutations.Add(mutation);
    }
}

public static class MutationTypes {
    public const string AddPercentage = "AddPercentage";
    public const string SubstractPercentage = "SubstractPercentage";
}