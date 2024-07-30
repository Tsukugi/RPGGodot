
using Godot;

public partial class AbilityBtn : Button {

    NavigationUnit linkedUnit = null;
    string linkedAbilityName = null;

    public override void _Ready() {
        base._Ready();
        ButtonUp += () => OnAbilityPressed(linkedAbilityName);
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


    void OnAbilityPressed(string name) {
        if (linkedUnit is null || linkedAbilityName is null) return;
        GD.Print("[OnAbilityPressed] " + linkedUnit.Name);
        if (linkedUnit.UnitCombat.Target is null) return;
        linkedUnit.CastAbility(name, linkedUnit.UnitCombat.Target);
    }
}