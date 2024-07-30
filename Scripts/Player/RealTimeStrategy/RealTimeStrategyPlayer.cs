
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class RealTimeStrategyPlayer : PlayerBase {
    RTSCamera rtsCamera;
    RTSSelection rtsSelection;
    RTSNavigation rtsNavigation;
    PackedScene navUnitTemplate = GD.Load<PackedScene>(ResourcePaths.NavigationUnit);

    RichTextLabel selectedUnitInfo;
    AbilityBtn abilityBtn;
    AbilityBtn abilityBtn2;

    public RTSNavigation RTSNavigation { get => rtsNavigation; }
    public RTSSelection RTSSelection { get => rtsSelection; }
    public RTSCamera RTSCamera { get => rtsCamera; }

    public override void _Ready() {
        base._Ready();
        rtsCamera = GetNodeOrNull<RTSCamera>(StaticNodePaths.PlayerInput);
        rtsSelection = GetNodeOrNull<RTSSelection>(StaticNodePaths.PlayerSelection);
        rtsNavigation = GetNodeOrNull<RTSNavigation>(StaticNodePaths.PlayerNavigation);
        selectedUnitInfo = GetNodeOrNull<RichTextLabel>(StaticNodePaths.PlayerUISelectedUnitInfo);
        abilityBtn = GetNodeOrNull<AbilityBtn>(StaticNodePaths.PlayerUIAbilityBtn);
        abilityBtn2 = GetNodeOrNull<AbilityBtn>(StaticNodePaths.PlayerUIAbilityBtn2);
        rtsSelection.OnSelectUnitsEvent += (selectedUnits) => {
            if (selectedUnits.Count == 0) {
                selectedUnitInfo.Text = "";
                return;
            }
            NavigationUnit unit = selectedUnits[0];
            List<string> abilitiesList = unit.Abilities.Keys.ToList();
            string abilities = string.Join(", ", abilitiesList);

            selectedUnitInfo.Text = "\n"
                + "Name: " + unit.Name + "\n"
                + "Abilities: [" + abilities + "] \n";

            if (abilitiesList.Count >= 1) abilityBtn.BindAbility(unit, abilitiesList[0]);
            if (abilitiesList.Count >= 2) abilityBtn2.BindAbility(unit, abilitiesList[1]);
        };
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