
using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class RTSSelection : Node {

    RealTimeStrategyPlayer player;

    // Selection
    List<NavigationUnit> selectedActors = new();
    SelectionPanel selectionPanel;
    Vector2 selectionAreaStart = Vector2.Zero;
    Vector2 selectionAreaEnd = Vector2.Zero;
    float minSelectionAreaForMultiSelection = 35;
    ShapeCast3D multiSelectionShapeCast3D;

    bool isSelecting = false;

    public Array CollisionsOnSelection { get => multiSelectionShapeCast3D.CollisionResult; }
    public List<NavigationUnit> SelectedActors { get => selectedActors; }
    public ShapeCast3D SelectionShapeCast3D { get => multiSelectionShapeCast3D; }


    public override void _Ready() {
        base._Ready();
        player = this.TryFindParentNodeOfType<RealTimeStrategyPlayer>();
        selectionPanel = player.GetNodeOrNull<SelectionPanel>(StaticNodePaths.PlayerUISelectionPanel);
        multiSelectionShapeCast3D = player.GetNodeOrNull<ShapeCast3D>(StaticNodePaths.PlayerMultiSelectionCast);
    }

    public override void _Input(InputEvent @event) {
        base._Input(@event);
        if (!player.IsFirstPlayer()) return;

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
                    multiSelectionShapeCast3D.CollideWithBodies = true;
                    selectionAreaStart = mousePosition;
                    selectionAreaEnd = selectionAreaStart;
                    UpdateSelectActorsInArea(selectionAreaStart, selectionAreaEnd);
                    UpdateSelectedActors(new List<NavigationUnit>());
                    player.DebugLog("[RTSSelection._Input]: Start Selection");
                } else {
                    // Reset
                    selectionAreaEnd = Vector2.Zero;
                    selectionAreaStart = Vector2.Zero;
                    selectionPanel.ResetPosition();
                    player.DebugLog("[RTSSelection._Input]: Selection Finished");
                    multiSelectionShapeCast3D.CollideWithBodies = false;
                }
            }
        }
    }


    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!player.IsFirstPlayer()) return;

        if (multiSelectionShapeCast3D.CollisionResult.Count > 0) {
            SelectionBase.SelectActors(UpdateSelectedActors, multiSelectionShapeCast3D.CollisionResult, player.Name);
        }
    }

    void UpdateSelectedActors(List<NavigationUnit> selectedUnits) {
        // TODO: Please improve me, i worry about this performance
        // Clear old and assign new selected state
        selectedActors.ForEach(actor => {
            try {
                actor.UnitSelection.IsSelected = false;
            } catch (System.ObjectDisposedException) {
                player.DebugLog("[UpdateSelectedActors] Tried to call a disposed actor, ignoring");
            }
        });
        player.DebugLog("[UpdateSelectedActors] Cleared actors: " + selectedActors.Count);
        selectedActors = selectedUnits;
        selectedActors.ForEach(actor => {
            actor.UnitSelection.IsSelected = true;
        });
        player.DebugLog("[UpdateSelectedActors] Added actors: " + selectedActors.Count);
    }

    void UpdateSelectActorsInArea(Vector2 selectionAreaStart, Vector2 selectionAreaEnd) {

        float selectionAreaSize = VectorUtils.GetDistanceFromVectors(selectionAreaStart, selectionAreaEnd);
        Vector2 areaStart = selectionAreaStart;
        Vector2 areaEnd = selectionAreaEnd;

        // If the selection size is too small (f.e. just a click) we will create an small area around it to select comfortably 
        if (selectionAreaSize < minSelectionAreaForMultiSelection) {
            Vector2 addedPadding = Vector2.One * minSelectionAreaForMultiSelection;
            areaStart = selectionAreaStart - addedPadding / 2;
            areaEnd = selectionAreaStart + addedPadding / 2;
            float newAreaSize = VectorUtils.GetDistanceFromVectors(areaStart, areaEnd);
            player.DebugLog("[RTSSelection.UpdateSelectActorsInArea]: selectionAreaSize " + newAreaSize);
        }

        Vector3 startWorldArea = player.RTSNavigation.NavigationBase.Get3DWorldPosition(player.Camera, areaStart);
        Vector3 startWorldArea2 = player.RTSNavigation.NavigationBase.Get3DWorldPosition(player.Camera, new(areaEnd.X, areaStart.Y));
        Vector3 endWorldArea = player.RTSNavigation.NavigationBase.Get3DWorldPosition(player.Camera, new(areaStart.X, areaEnd.Y));
        Vector3 endWorldArea2 = player.RTSNavigation.NavigationBase.Get3DWorldPosition(player.Camera, areaEnd);

        /* We are drawing a "cube" with Y -10, 10 */
        ((ConvexPolygonShape3D)multiSelectionShapeCast3D.Shape).Points = new Vector3[] {
                startWorldArea.WithY(-10), startWorldArea2.WithY(-10), endWorldArea.WithY(-10), endWorldArea2.WithY(-10),
                startWorldArea.WithY(10), startWorldArea2.WithY(10), endWorldArea.WithY(10), endWorldArea2.WithY(10)
            };
    }
}