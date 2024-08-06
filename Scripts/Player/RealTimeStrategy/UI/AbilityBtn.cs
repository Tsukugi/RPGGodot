
using Godot;

public partial class AbilityBtn : Button {
    PlayerBase player;

    bool hasAbilityBound = false;
    Unit linkedUnit = null;
    string linkedAbilityName = null;

    [Export]
    Key assignedKey;

    public override void _Ready() {
        base._Ready();
        player = this.TryFindParentNodeOfType<PlayerBase>();
        ButtonUp += OnAbilityPressed;
    }



    public override void _Input(InputEvent @event) {
        base._Input(@event);
        // if (!player.CanInteract(PlayerInteractionType.AbilityCast)) return
        if (@event is InputEventKey keyEvent) {
            if (keyEvent.Pressed && keyEvent.Keycode == assignedKey) {
                OnAbilityPressed();
            }
        }
    }

    public void BindAbility(Unit unit, string abilityName) {
        if (!unit.Player.IsFirstPlayer()) return;
        linkedUnit = unit;
        linkedAbilityName = abilityName;

        Visible = true;
        Text = linkedAbilityName;
        hasAbilityBound = true;
    }

    public void UnbindAbility() {
        Visible = false;
        Text = "";

        linkedUnit = null;
        linkedAbilityName = null;
        hasAbilityBound = false;
    }


    void OnAbilityPressed() {
        if (!hasAbilityBound) return;
        linkedUnit.CastAbility(linkedAbilityName);
    }
}