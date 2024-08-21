using Godot;

public partial class VSUIMutatorSelector : Node {
    UnitAttributeMutationDTO positiveMutation;
    UnitAttributeMutationDTO negativeMutation;
    public delegate void OnMutatorSelect(UnitAttributeMutationDTO positive, UnitAttributeMutationDTO negative);
    public event OnMutatorSelect OnMutatorSelected;

    bool hasData = false;

    Label label;
    Button selectBtn;

    public override void _Ready() {
        base._Ready();
        label = GetNode<Label>("Label");
        selectBtn = GetNode<Button>("Button");
        selectBtn.Pressed += () => OnMutatorSelected?.Invoke(positiveMutation, negativeMutation);
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!hasData) return;
        label.Text = $"{GetMutationText(positiveMutation)} \n {GetMutationText(negativeMutation, false)}";
    }

    public void UpdateMutations(UnitAttributeMutationDTO positiveMutation, UnitAttributeMutationDTO negativeMutation) {
        this.positiveMutation = positiveMutation;
        this.negativeMutation = new UnitAttributeMutationDTO() {
            id = negativeMutation.id.Replace("Add", "Sub"),
            attributeName = negativeMutation.attributeName,
            mutationType = negativeMutation.mutationType,
            value = negativeMutation.value * -1 // ! This makes it to be negative
        };

        GD.Print($"+{this.positiveMutation.id} -{this.negativeMutation.id}");
        hasData = true;
    }

    // ! TODO Add i18ns
    static string GetMutationText(UnitAttributeMutationDTO mutation, bool positive = true) {
        string verb = positive ? "Increases" : "Decreases";
        return mutation.mutationType switch {
            MutationTypes.AddFixed => $"{verb} {mutation.value} to your {mutation.attributeName}.",
            MutationTypes.AddPercentage => $"{verb} your {mutation.attributeName} by {mutation.value}%.",
            _ => $"{verb} {mutation.value} to your {mutation.attributeName}.",
        };
    }
}