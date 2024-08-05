using System.Collections.Generic;
using Godot;

public partial class PlayerSelection : Node {
    protected ShapeCast3D selectionShapeCast3D;
    protected PlayerBase player;
    protected readonly EnvironmentBase envBase = new();
    protected List<Unit> selectedUnits = new();
    protected Vector2 selectionAreaStart = Vector2.Zero;
    protected Vector2 selectionAreaEnd = Vector2.Zero;
    protected float minSelectionAreaForMultiSelection = 35;

    public PlayerInteractionType AllowedInteractionType = PlayerInteractionType.None;
    public delegate void SelectionEvent(List<Unit> selectedActors);
    public event SelectionEvent OnSelectUnitsEvent;

    public override void _Ready() {
        base._Ready();
        player = this.TryFindParentNodeOfType<PlayerBase>();
        selectionShapeCast3D = GetNode<ShapeCast3D>(StaticNodePaths.SelectionCast);
    }

    // TODO I think we can do it event based and NOT physicsProcess based. 
    public void CheckCollisionResult() {
        if (!player.IsFirstPlayer()) return;
        if (AllowedInteractionType == PlayerInteractionType.None) {
            GD.PushWarning("[PlayerSelection] Please set a AllowedInteractionType, ignoring all _PhysicsProcess otherwise.");
            return;
        } else if (player.CanInteract(AllowedInteractionType)) {
            selectionShapeCast3D.ForceShapecastUpdate();
            if (selectionShapeCast3D.CollisionResult.Count > 0) {
                SelectionUtils.SelectActors(UpdateSelectedActors, selectionShapeCast3D.CollisionResult, player.Name);
            }
        }
    }

    public virtual void StartSelection(Vector2 startPosition) {
        selectionShapeCast3D.CollideWithBodies = true;
        selectionAreaStart = startPosition;
        selectionAreaEnd = selectionAreaStart;
        UpdateCastArea();
        CheckCollisionResult();
        player.DebugLog("[PlayerSelection]: Start Selection", true);
    }

    public virtual void UpdateSelectionArea(Vector2 selectionAreaStart, Vector2 selectionAreaEnd) {
        this.selectionAreaStart = selectionAreaStart;
        this.selectionAreaEnd = selectionAreaEnd;
        UpdateCastArea();
        CheckCollisionResult();
    }

    public virtual void EndSelection() {
        selectionAreaEnd = Vector2.Zero;
        selectionAreaStart = Vector2.Zero;
        selectionShapeCast3D.CollideWithBodies = false;
        player.DebugLog("[PlayerSelection]: Selection Finished", true);
    }

    protected void UpdateCastArea() {
        float selectionAreaSize = selectionAreaStart.DistanceTo(selectionAreaEnd);
        Vector2 areaStart = selectionAreaStart;
        Vector2 areaEnd = selectionAreaEnd;

        // If the selection size is too small (f.e. just a click) we will create an small area around it to select comfortably 
        if (selectionAreaSize < minSelectionAreaForMultiSelection) {
            Vector2 addedPadding = Vector2.One * minSelectionAreaForMultiSelection;
            areaStart = selectionAreaStart - addedPadding / 2;
            areaEnd = selectionAreaStart + addedPadding / 2;
            float newAreaSize = areaStart.DistanceTo(areaEnd);
            player.DebugLog("[RTSSelection.UpdateSelectActorsInArea]: selectionAreaSize " + newAreaSize);
        }

        Vector3 startWorldArea = envBase.Get3DWorldPosition(player.Camera, areaStart);
        Vector3 startWorldArea2 = envBase.Get3DWorldPosition(player.Camera, new(areaEnd.X, areaStart.Y));
        Vector3 endWorldArea = envBase.Get3DWorldPosition(player.Camera, new(areaStart.X, areaEnd.Y));
        Vector3 endWorldArea2 = envBase.Get3DWorldPosition(player.Camera, areaEnd);

        /* We are drawing a "cube" with Y -10, 10 */
        ((ConvexPolygonShape3D)selectionShapeCast3D.Shape).Points = new Vector3[] {
                startWorldArea.WithY(-10), startWorldArea2.WithY(-10), endWorldArea.WithY(-10), endWorldArea2.WithY(-10),
                startWorldArea.WithY(10), startWorldArea2.WithY(10), endWorldArea.WithY(10), endWorldArea2.WithY(10)
            };
    }
    protected virtual void UpdateSelectedActors(List<Unit> selectedUnits) {
        this.selectedUnits = selectedUnits;
        OnSelectUnitsEvent?.Invoke(this.selectedUnits);
        player.DebugLog("[UpdateSelectedActors] New actor count: " + this.selectedUnits.Count);
    }

}