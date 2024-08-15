
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class RealTimeStrategyPlayer : PlayerBase {
    RTSCamera rtsCamera;
    RTSSelection rtsSelection;
    RTSNavigation rtsNavigation;
    RTSUI rtsUI;
    PackedScene navUnitTemplate = GD.Load<PackedScene>(ResourcePaths.NavigationUnit);
    RichTextLabel selectedUnitInfo;
    PlayerInteractionType defaultInterationType = PlayerInteractionType.UnitControl;

    public RTSNavigation RTSNavigation { get => rtsNavigation; }
    public RTSSelection RTSSelection { get => rtsSelection; }
    public RTSCamera RTSCamera { get => rtsCamera; }

    public override void _Ready() {
        base._Ready();
        rtsCamera = GetNode<RTSCamera>(StaticNodePaths.PlayerRTSCamera);
        rtsSelection = GetNode<RTSSelection>(StaticNodePaths.PlayerRTSSelection);
        rtsNavigation = GetNode<RTSNavigation>(StaticNodePaths.PlayerRTSNavigation);
        rtsUI = GetNode<RTSUI>(StaticNodePaths.PlayerRTSUI);
        selectedUnitInfo = GetNode<RichTextLabel>(StaticNodePaths.PlayerUISelectedUnitInfo);
        rtsSelection.OnSelectUnitsEvent -= OnSelectUnitsEvent;
        rtsSelection.OnSelectUnitsEvent += OnSelectUnitsEvent;
        if (this.IsFirstPlayer()) StartInteractionType(PlayerInteractionType.UnitControl);
    }

    public List<NavigationUnit> GetAllUnits() {
        return this.TryGetAllChildOfType<NavigationUnit>();
    }

    public NavigationUnit AddUnit(UnitDTO unitDTO, Vector3 position) {
        NavigationUnit navUnit = navUnitTemplate.Instantiate<NavigationUnit>();
        AddChild(navUnit);
        navUnit.UnitAttributes.InitializeValues(unitDTO.attributes);
        navUnit.Name = unitDTO.name;
        navUnit.Position = position;
        foreach (string item in unitDTO.abilities) {
            navUnit.AddAbility(PlayerManager.Database.Abilites[item]);
        }

        return navUnit;
    }

    public override void StopInteraction() {
        StartInteractionType(defaultInterationType);
    }

    void OnSelectUnitsEvent(List<Unit> selectedUnits) {
        //! Debug
        if (selectedUnits.Count == 0) {
            selectedUnitInfo.Text = "";
            return;
        }
        Unit unit = selectedUnits[0];
        List<string> abilitiesList = unit.Abilities.Keys.ToList();
        string abilities = string.Join(", ", abilitiesList);

        selectedUnitInfo.Text = "\n"
            + "Name: " + unit.Name + "\n"
            + "Abilities: [" + abilities + "] \n";

        for (int i = 0; i < rtsUI.AbilityButtons.Count; i++) {
            rtsUI.AbilityButtons[i].UnbindAbility();
            if (abilitiesList.Count >= i + 1) rtsUI.AbilityButtons[i].BindAbility(unit, abilitiesList[i]);
        }
        //! DebugEnd
    }
}