
using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class RTSSelection : Node {

    RealTimeStrategyPlayer player;

    // Selection
    List<NavigationUnit> selectedUnits = new();
    SelectionPanel selectionPanel;
    Vector2 selectionAreaStart = Vector2.Zero;
    Vector2 selectionAreaEnd = Vector2.Zero;
    float minSelectionAreaForMultiSelection = 35;

    bool isSelecting = false;

    public Array CollisionsOnSelection { get => selectionShapeCast3D.CollisionResult; }
    public List<NavigationUnit> SelectedUnits { get => selectedUnits; }
    public ShapeCast3D SelectionShapeCast3D { get => selectionShapeCast3D; }


    public delegate void SelectionEvent(List<NavigationUnit> selectedActors);
    public event SelectionEvent OnSelectUnitsEvent;

    public override void _Ready() {
        base._Ready();
        player = this.TryFindParentNodeOfType<RealTimeStrategyPlayer>();
        selectionPanel = player.GetNodeOrNull<SelectionPanel>(StaticNodePaths.PlayerUISelectionPanel);
        selectionShapeCast3D = player.GetNodeOrNull<ShapeCast3D>(StaticNodePaths.PlayerSelectionCast);
    }

    public override void _Input(InputEvent @event) {
        base._Input(@event);
        if (!player.IsFirstPlayer() || !player.CanInteract(PlayerInteractionType.Selection)) return;

        Vector2 mousePosition = player.Camera.GetViewport().GetMousePosition();

        if (@event is InputEventMouseMotion) {
            if (Input.IsMouseButtonPressed(MouseButton.Left)) {
                selectionAreaEnd = mousePosition;
                selectionPanel.ApplySelectionTransform(selectionAreaStart, selectionAreaEnd);
                UpdateSelectActorsInArea(selectionAreaStart, selectionAreaEnd);
            }
        }

        if (@event is InputEventMouseButton eventMouseButton) {
            if (eventMouseButton.ButtonIndex == MouseButton.Left) {
                // The Selecting phase is since the user presses the mouse button until he releases it
                if (eventMouseButton.Pressed) {
                    selectionShapeCast3D.CollideWithBodies = true;
                    selectionAreaStart = mousePosition;
                    selectionAreaEnd = selectionAreaStart;
                    UpdateSelectActorsInArea(selectionAreaStart, selectionAreaEnd);
                    // UpdateSelectedActors(new List<NavigationUnit>());
                    player.DebugLog("[RTSSelection._Input]: Start Selection");
                } else {
                    // Reset
                    selectionAreaEnd = Vector2.Zero;
                    selectionAreaStart = Vector2.Zero;
                    selectionPanel.ResetPosition();
                    player.DebugLog("[RTSSelection._Input]: Selection Finished");
                    selectionShapeCast3D.CollideWithBodies = false;
                }
            }
        }
    }


    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!player.IsFirstPlayer()) return;

        if (selectionShapeCast3D.CollisionResult.Count > 0) {
            SelectionUtils.SelectActors(UpdateSelectedActors, selectionShapeCast3D.CollisionResult, player.Name);
        }
    }

    void UpdateSelectedActors(List<NavigationUnit> selectedUnits) {
        // TODO: Please improve me, i worry about this performance
        // Clear old and assign new selected state
        this.selectedUnits.ForEach(actor => {
            try {
                actor.UnitSelection.Deselect();
            } catch (System.ObjectDisposedException) {
                player.DebugLog("[UpdateSelectedActors] Tried to call a disposed actor, ignoring");
            }
        });
        player.DebugLog("[UpdateSelectedActors] Cleared actors: " + this.selectedUnits.Count);
        this.selectedUnits = selectedUnits;
        this.selectedUnits.ForEach(actor => {
            actor.UnitSelection.Select(player);
        });
        OnSelectUnitsEvent?.Invoke(this.selectedUnits);
        player.DebugLog("[UpdateSelectedActors] Added actors: " + this.selectedUnits.Count);
    }

   
}