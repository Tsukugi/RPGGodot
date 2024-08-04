
using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class RTSSelection : PlayerSelection {
    new RealTimeStrategyPlayer player;

    // Selection
    SelectionPanel selectionPanel;
    bool isSelecting = false;
    public Array CollisionsOnSelection { get => selectionShapeCast3D.CollisionResult; }
    public List<Unit> SelectedUnits { get => selectedUnits; }
    public ShapeCast3D SelectionShapeCast3D { get => selectionShapeCast3D; }

    public override void _Ready() {
        base._Ready();
        player = this.TryFindParentNodeOfType<RealTimeStrategyPlayer>();
        selectionPanel = player.GetNode<SelectionPanel>(StaticNodePaths.PlayerUISelectionPanel);
        AllowedInteractionType = PlayerInteractionType.UnitControl;
    }

    public override void _Input(InputEvent @event) {
        base._Input(@event);
        if (!player.IsFirstPlayer()) return;
        if (!player.CanInteract(PlayerInteractionType.UnitControl)) return;

        Vector2 mousePosition = player.Camera.GetViewport().GetMousePosition();

        if (@event is InputEventMouseMotion) {
            if (Input.IsMouseButtonPressed(MouseButton.Left)) {
                selectionAreaEnd = mousePosition;
                selectionPanel.ApplySelectionTransform(selectionAreaStart, selectionAreaEnd);
                UpdateCastArea();
            }
        }

        if (@event is InputEventMouseButton eventMouseButton) {
            if (eventMouseButton.ButtonIndex == MouseButton.Left) {
                // The Selecting phase is since the user presses the mouse button until he releases it
                if (eventMouseButton.Pressed) {
                    StartSelection(mousePosition);
                } else {
                    EndSelection();
                }
            }
        }
    }

    public override void StartSelection(Vector2 startPosition) {
        base.StartSelection(startPosition);
        UpdateSelectedActors(new List<Unit>());
    }

    public override void EndSelection() {
        base.EndSelection();
        selectionPanel.ResetPosition();
    }

    protected override void UpdateSelectedActors(List<Unit> selectedUnits) {
        // TODO: Please improve me, i worry about this performance
        // Clear old and assign new selected state
        this.selectedUnits.ForEach(actor => {
            try {
                if (actor is not NavigationUnit navUnit) return;
                navUnit.UnitSelection.Deselect();
            } catch (System.ObjectDisposedException) {
                player.DebugLog("[UpdateSelectedActors] Tried to call a disposed actor, ignoring");
            }
        });
        player.DebugLog("[UpdateSelectedActors] Cleared actors: " + this.selectedUnits.Count);
        base.UpdateSelectedActors(selectedUnits);
        this.selectedUnits.ForEach(actor => {
            if (actor is not NavigationUnit navUnit) return;
            navUnit.UnitSelection.Select(player);
        });
        player.DebugLog("[UpdateSelectedActors] Added actors: " + this.selectedUnits.Count);
    }
}