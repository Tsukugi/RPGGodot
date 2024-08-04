using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class PlayerAbility : Node {
    PlayerBase player;
    AbilityCaster abilityCaster = null;
    AbilityCastContext castContext = null;
    MeshInstance3D abilityIndicator;
    protected PlayerSelection playerSelection;
    readonly EnvironmentBase envBase = new();
    public override void _Ready() {
        base._Ready();
        player = this.TryFindParentNodeOfType<PlayerBase>();
        abilityIndicator = GetNode<MeshInstance3D>(StaticNodePaths.AbilityIndicator);
        playerSelection = GetNode<PlayerSelection>(StaticNodePaths.PlayerSelection);
        playerSelection.AllowedInteractionType = PlayerInteractionType.AbilityCast;
        playerSelection.OnSelectUnitsEvent += OnSelectedTargetUnits;
    }

    public override void _Input(InputEvent @event) {
        base._Input(@event);
        if (!player.IsFirstPlayer()) return;
        if (!player.CanInteract(PlayerInteractionType.AbilityCast)) return;

        Vector2 mousePosition = player.Camera.GetViewport().GetMousePosition();
        switch (castContext.Type) {
            case AbilityCastTypes.Target: OnInputCastTargetAbility(@event, mousePosition); break;
            case AbilityCastTypes.Position: OnInputCastPositionAbility(@event, mousePosition); break;
        }
    }

    // * Input events for a Targeted ability
    void OnInputCastTargetAbility(InputEvent @event, Vector2 mousePosition) {
        Vector3 mousePositionInWorld = envBase.Get3DWorldPosition(player.Camera, mousePosition);

        if (@event is InputEventMouseMotion) {
            abilityIndicator.GlobalPosition = mousePositionInWorld;
            playerSelection.UpdateSelectionArea(mousePosition, mousePosition);
        }

        if (@event is InputEventMouseButton eventMouseButton) {
            if (eventMouseButton.Pressed) {
                if (eventMouseButton.ButtonIndex == MouseButton.Left) {
                    castContext.AddTargetPosition(mousePositionInWorld);
                    playerSelection.StartSelection(mousePosition);
                } else if (eventMouseButton.ButtonIndex == MouseButton.Right) {
                    EndCastingState();
                }
            }

        }
    }
    // This is the result of the selection event, thus finishing the Ability casting on a target
    // TODO: What do we do with multiple units?
    void OnSelectedTargetUnits(List<Unit> units) {
        if (castContext is null) GD.PrintErr("[OnSelectedTargetUnits] No context specified.");
        if (units.Count > 0) {
            Unit target = units[0];
            player.DebugLog("[OnSelectedTargetUnits] Target found " + target.Name, true);
            castContext.AddTarget(target);
            abilityCaster.Cast(castContext);
            EndCastingState();
        } else {
            player.DebugLog("[OnSelectedTargetUnits] No units found", true);
            playerSelection.EndSelection();
        }
    }

    // * Input events for a Positioned ability
    void OnInputCastPositionAbility(InputEvent @event, Vector2 mousePosition) {
        Vector3 mousePositionInWorld = envBase.Get3DWorldPosition(player.Camera, mousePosition);
        if (@event is InputEventMouseMotion) {
            abilityIndicator.GlobalPosition = mousePositionInWorld;
        }

        if (@event is InputEventMouseButton eventMouseButton) {
            if (eventMouseButton.ButtonIndex == MouseButton.Left) {
                castContext.AddTargetPosition(mousePositionInWorld);
                abilityCaster.Cast(castContext);
            }
            EndCastingState();
        }
    }

    public void StartCastingState(AbilityCaster abilityCaster) {
        GetTree().Paused = true;
        player.StartInteractionType(PlayerInteractionType.AbilityCast);
        castContext = new(abilityCaster.Ability.attributes.castType);
        this.abilityCaster = abilityCaster;
        player.DebugLog("[StartCastingState] " + castContext, true);
    }
    public void EndCastingState() {
        GetTree().Paused = false;
        playerSelection.EndSelection();
        player.DebugLog("[EndCastingState]", true);
        player.StopInteraction();
        castContext = null;
        abilityCaster = null;
    }

}
