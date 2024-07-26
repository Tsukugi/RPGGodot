
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class RealTimeStrategyPlayer : PlayerBase {
    RTSCamera rtsCamera;
    RTSSelection rtsSelection;
    RTSNavigation rtsNavigation;
    PackedScene navUnitTemplate = GD.Load<PackedScene>(ResourcePaths.NavigationUnit);

    RichTextLabel selectedUnitInfo;
    Button abilityBtn;
    Button abilityBtn2;

    public RTSNavigation RTSNavigation { get => rtsNavigation; }
    public RTSSelection RTSSelection { get => rtsSelection; }
    public RTSCamera RTSCamera { get => rtsCamera; }

    public override void _Ready() {
        base._Ready();
        rtsCamera = GetNodeOrNull<RTSCamera>(StaticNodePaths.PlayerInput);
        rtsSelection = GetNodeOrNull<RTSSelection>(StaticNodePaths.PlayerSelection);
        rtsNavigation = GetNodeOrNull<RTSNavigation>(StaticNodePaths.PlayerNavigation);
        selectedUnitInfo = GetNodeOrNull<RichTextLabel>(StaticNodePaths.PlayerUISelectedUnitInfo);
        abilityBtn = GetNodeOrNull<Button>(StaticNodePaths.PlayerUIAbilityBtn);
        abilityBtn2 = GetNodeOrNull<Button>(StaticNodePaths.PlayerUIAbilityBtn2);

        rtsSelection.OnSelectUnitsEvent += (selectedUnits) => {
            if (selectedUnits.Count == 0) {
                selectedUnitInfo.Text = "";
                return;
            }
            var unit = selectedUnits[0];
            var abilitiesList = unit.Abilities.Keys.ToList();
            string abilities = string.Join(", ", abilitiesList);

            selectedUnitInfo.Text = "Name: " + unit.Name + "\n"
            + "Abilities: [" + abilities + "] \n";
            if (abilitiesList.Count >= 1) BindButtonToAbility(unit, unit.Abilities[abilitiesList[0]], abilityBtn);
            if (abilitiesList.Count >= 2) BindButtonToAbility(unit, unit.Abilities[abilitiesList[1]], abilityBtn2);
        };
    }

    void BindButtonToAbility(NavigationUnit unit, AbilityCaster ability, Button btn) {
        if (!this.IsFirstPlayer()) return;
        btn.Visible = true;
        btn.Text = ability.AbilityAttributes.name;
        btn.Pressed += () => OnAbilityPressed(unit, ability.AbilityAttributes.name);
    }

    void OnAbilityPressed(NavigationUnit unit, string name) {
        if (!this.IsFirstPlayer()) return;
        DebugLog("[abilityBtn.Pressed] " + unit.Name, true);
        if (unit.UnitCombat.Target is null) return;
        unit.CastAbility(name, unit.UnitCombat.Target);
    }

    public List<NavigationUnit> GetAllUnits() {
        return this.TryGetAllChildOfType<NavigationUnit>();
    }

    public void AddUnit(NavigationUnitDTO unitDTO, Vector3 position) {
        NavigationUnit navUnit = navUnitTemplate.Instantiate<NavigationUnit>();
        AddChild(navUnit);
        navUnit.Attributes.UpdateValues(unitDTO.attributes);
        navUnit.Name = unitDTO.name;
        navUnit.Position = position;
        foreach (string item in unitDTO.abilities) {
            navUnit.AddAbility(LocalDatabase.Abilites[item]);
        }
    }
}