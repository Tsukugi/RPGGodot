using System.Collections.Generic;

public class UnitMutableAttributes : UnitAttributes {
    List<UnitAttributeMutation> mutations = new();

    public UnitMutableAttributes(Unit unit) : base(unit) { }

    public List<UnitAttributeMutation> Mutations { get => mutations; }

    public new AttributesExport GetAttributes() {
        return ApplyMutations(base.GetAttributes());
    }

    public AttributesExport ApplyMutations(AttributesExport attributes) {
        mutations.ForEach(mutation => {
            attributes.SetObjectProperty(mutation.attributeName, mutation.value);
        });
        return attributes;
    }

    public void AddMutation(UnitAttributeMutation mutation) {
        mutations.Add(mutation);
    }
}

public class UnitAttributeMutation : DTOBase {
    public string attributeName;
    public string attributeMutationType;
    public float value;
}