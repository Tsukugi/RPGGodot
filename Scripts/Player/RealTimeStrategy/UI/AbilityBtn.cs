
using Godot;

public partial class AbilityBtn : Button {
    bool isInitialized = false;
    NavigationUnit linkedUnit = null;
    string linkedAbilityName = null;

    public override void _Ready() {
        base._Ready();
        ButtonUp += OnAbilityPressed;
        isInitialized = true;
    }

    public void BindAbility(NavigationUnit unit, string abilityName) {
        if (!unit.Player.IsFirstPlayer()) return;
        linkedUnit = unit;
        linkedAbilityName = abilityName;

        Visible = true;
        Text = linkedAbilityName;
    }

    public void UnbindAbility() {
        Visible = false;
        Text = "";

        linkedUnit = null;
        linkedAbilityName = null;
    }


    void OnAbilityPressed() {
        if (!isInitialized) return;
        linkedUnit.CastAbility(linkedAbilityName);
    }
}