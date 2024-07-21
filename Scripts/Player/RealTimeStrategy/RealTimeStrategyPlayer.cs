
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
        abilityBtn.Pressed += OnAbilityPressed;
    }


    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (rtsSelection.SelectedActors.Count == 0) {
            selectedUnitInfo.Text = "";
            return;
        }
        var unit = rtsSelection.SelectedActors[0];
        string abilities = string.Join(", ", unit.Abilities.Keys.ToList());

        selectedUnitInfo.Text = "Name: " + unit.Name + "\n"
        + "Abilities: [" + abilities + "] \n";
    }


    void OnAbilityPressed() {
        if (!this.IsFirstPlayer()) return;
        DebugLog("[abilityBtn.Pressed]", true);
        if (rtsSelection.SelectedActors.Count == 0) return;
        var unit = rtsSelection.SelectedActors[0];
        DebugLog("[abilityBtn.Pressed] " + unit.Name, true);
        if (unit.UnitCombat.Target is null) return;
        unit.CastAbility("Fireball", unit.UnitCombat.Target);
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
            navUnit.AddAbility(PlayerManager.localDatabase.Abilites[item]);
        }
    }
}