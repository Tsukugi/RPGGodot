using Godot;

public partial class PlayerAbility : Node {
    PlayerBase player;
    AbilityCastContext abilityCastContext = null;
    MeshInstance3D abilityIndicator;
    readonly NavigationBase navBase = new(); // TODO Change NavBase to not be related to navigation, we just wanna 2D mouse coordinates to World coordinates!
    public override void _Ready() {
        base._Ready();
        player = this.TryFindParentNodeOfType<PlayerBase>();
        abilityIndicator = player.GetNodeOrNull<MeshInstance3D>(StaticNodePaths.PlayerAbility_AbilityIndicator);
    }

    public override void _Input(InputEvent @event) {
        base._Input(@event);
        if (!player.IsFirstPlayer()) return;
        OnAbilityCastInput(@event);
    }


    void OnAbilityCastInput(InputEvent @event) {
        if (!player.CanInteract(PlayerInteractionType.AbilityCast)) return;

        Vector2 mousePosition = player.Camera.GetViewport().GetMousePosition();
        Vector3 mousePositionInWorld = navBase.Get3DWorldPosition(player.Camera, mousePosition); ;
        switch (abilityCastContext.Type) {
            case AbilityCastTypes.Target: OnTargetAbilityCastInput(@event, mousePositionInWorld); break;
            case AbilityCastTypes.Position: OnTargetPositionAbilityCastInput(@event, mousePositionInWorld); break;
        }
    }

    void OnTargetAbilityCastInput(InputEvent @event, Vector3 mousePositionInWorld) {
        if (@event is InputEventMouseMotion) {
            abilityIndicator.GlobalPosition = mousePositionInWorld;
        }

        if (@event is InputEventMouseButton eventMouseButton) {
            EndCastingState();
            if (eventMouseButton.ButtonIndex == MouseButton.Left) {
                abilityCastContext.AddTargetPosition(mousePositionInWorld);
                abilityCastContext.AddTarget();
                abilityCastContext.AbilityCaster.Cast();
            }
        }
    }

    void OnTargetPositionAbilityCastInput(InputEvent @event, Vector3 mousePositionInWorld) {
        if (@event is InputEventMouseMotion) {
            abilityIndicator.GlobalPosition = mousePositionInWorld;
        }

        if (@event is InputEventMouseButton eventMouseButton) {
            EndCastingState();
            if (eventMouseButton.ButtonIndex == MouseButton.Left) {
                abilityCastContext.AddTargetPosition(mousePositionInWorld);
                abilityCastContext.AbilityCaster.Cast();
            }
        }
    }

    public void StartCastingState(AbilityCaster abilityCaster) {
        player.DebugLog("[StartCastingState]", true);
        player.Interact(PlayerInteractionType.AbilityCast);
        abilityCastContext = new(abilityCaster);
        GetTree().Paused = true;
    }
    public void EndCastingState() {
        player.DebugLog("[EndCastingState]", true);
        player.StopInteraction();
        abilityCastContext = null;
        GetTree().Paused = false;
    }

}
