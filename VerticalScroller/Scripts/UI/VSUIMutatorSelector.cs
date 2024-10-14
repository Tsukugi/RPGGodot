using System;
using Godot;

public partial class VSUIMutatorSelector : Node {
    MutationDTO positiveMutation;
    MutationDTO negativeMutation;
    public delegate void OnMutatorSelect(MutationDTO positive, MutationDTO negative);
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

    static double GetMutatorVariation() {
        return 0.5f + new Random().NextDouble(); // Should be from 0.5 to 1.5
    }

    public void UpdateMutations(MutationDTO positiveMutation, MutationDTO negativeMutation) {
        this.positiveMutation = new MutationDTO() {
            id = positiveMutation.id,
            attributeName = positiveMutation.attributeName,
            mutationType = positiveMutation.mutationType,
            mutationTarget = positiveMutation.mutationTarget,
            value = Math.Round(positiveMutation.value * GetMutatorVariation(), 1)
        }; ;
        this.negativeMutation = new MutationDTO() {
            id = negativeMutation.id.Replace("Add", "Sub"),
            attributeName = negativeMutation.attributeName,
            mutationType = negativeMutation.mutationType,
            mutationTarget = negativeMutation.mutationTarget,
            value = Math.Round(negativeMutation.value * GetMutatorVariation() * -1, 1) // ! This makes it to be negative
        };

        GD.Print($"+{this.positiveMutation.id} -{this.negativeMutation.id}");
        hasData = true;
    }

    // ! TODO Add i18ns
    static string GetMutationText(MutationDTO mutation, bool positive = true) {
        string verb = positive ? "Increases" : "Decreases";
        return mutation.mutationType switch {
            MutationTypes.AddFixed => $"{verb} {mutation.value} to your {mutation.attributeName}.",
            MutationTypes.AddPercentage => $"{verb} your {mutation.attributeName} by {mutation.value}%.",
            _ => $"{verb} {mutation.value} to your {mutation.attributeName}.",
        };
    }
}