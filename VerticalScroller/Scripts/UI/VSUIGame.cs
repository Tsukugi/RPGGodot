using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class VSUIGame : VSUIBase {
    VSGameManager gameManager;

    HBoxContainer mutatorSelection;
    Label attributesLabel;
    const string btnPath = "";
    readonly List<string> btnNames = new() { "BackBtn" };

    public override void _Ready() {
        base._Ready();
        gameManager = this.TryFindParentNodeOfType<VSGameManager>();
        mutatorSelection = GetNode<HBoxContainer>("MutatorSelectionContainer");
        attributesLabel = GetNode<Label>("Attributes");
        AttachEvents(btnNames, btnPath);
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        RenderAttributes();
    }


    void RenderAttributes() {
        AttributesExport attributes = gameManager.PlayerUnit.GetAttributes();
        AttributesExport mutations = gameManager.PlayerUnit.UnitAttributes.GetMutations();

        var mutationIds = gameManager.PlayerUnit.UnitAttributes.Mutations.Select(m => m.id).ToArray();
        var mutatorsText = string.Join(",", mutationIds);

        attributesLabel.Text = $"Attributes: \n" +
            $"Max HP: {attributes.MaxHitPoints} {GetMutationText(mutations.MaxHitPoints)}\n" +
            $"HP: {attributes.HitPoints} {GetMutationText(mutations.HitPoints)}\n" +
            $"Armor: {attributes.Armor} {GetMutationText(mutations.Armor)}\n" +
            $"Base Damage: {attributes.BaseDamage} {GetMutationText(mutations.BaseDamage)}\n" +
            $"Attack Speed: {attributes.AttackSpeed} {GetMutationText(mutations.AttackSpeed)}\n" +
            $"Mutators: {mutatorsText}\n";
    }

    public static string GetMutationText(double value) {
        string result = ".";
        string plusSign = value > 0 ? "+" : ""; // We want to give +N if positive
        if (value == 0) return result;
        else result = $"({plusSign}{value})";

        return result;
    }

    public bool SetMutatorSelectionVisibility(bool visible) {
        mutatorSelection.Visible = visible;
        return mutatorSelection.Visible;
    }
}