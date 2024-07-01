
using System.Collections.Generic;
using Godot;

public partial class RealTimeStrategyPlayer : PlayerBase {
    RTSCamera rtsCamera;
    RTSSelection rtsSelection;
    RTSNavigation rtsNavigation;
    PackedScene navUnitTemplate = GD.Load<PackedScene>(ResourcePaths.NavigationUnit);

    public RTSNavigation RTSNavigation { get => rtsNavigation; }
    public RTSSelection RTSSelection { get => rtsSelection; }
    public RTSCamera RTSCamera { get => rtsCamera; }

    public override void _Ready() {
        base._Ready();
        rtsCamera = GetNodeOrNull<RTSCamera>(StaticNodePaths.PlayerInput);
        rtsSelection = GetNodeOrNull<RTSSelection>(StaticNodePaths.PlayerSelection);
        rtsNavigation = GetNodeOrNull<RTSNavigation>(StaticNodePaths.PlayerNavigation);
    }

    public List<NavigationUnit> GetAllUnits() {
        return this.TryGetAllChildOfType<NavigationUnit>();
    }

    public void AddUnits(Vector3 position, int amount = 1) {
        for (int i = 0; i < amount; i++) {
            NavigationUnit navUnit = navUnitTemplate.Instantiate<NavigationUnit>();
            AddChild(navUnit);
            navUnit.Position = position;
            navUnit.NavigationAgent.StartNewNavigation(position.Add(1));
        }
    }
}