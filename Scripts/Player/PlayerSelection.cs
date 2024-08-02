using Godot;

public partial class PlayerSelection : Node {
    ShapeCast3D selectionShapeCast3D;

    // TODO Make a Node module of this
    public void UpdateSelectActorsInArea(Vector2 selectionAreaStart, Vector2 selectionAreaEnd, float minSelectionAreaForMultiSelection, PlayerBase player) {

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

        Vector3 startWorldArea = Get3DWorldPosition(player.Camera, areaStart);
        Vector3 startWorldArea2 = Get3DWorldPosition(player.Camera, new(areaEnd.X, areaStart.Y));
        Vector3 endWorldArea = Get3DWorldPosition(player.Camera, new(areaStart.X, areaEnd.Y));
        Vector3 endWorldArea2 = Get3DWorldPosition(player.Camera, areaEnd);

        /* We are drawing a "cube" with Y -10, 10 */
        ((ConvexPolygonShape3D)selectionShapeCast3D.Shape).Points = new Vector3[] {
                startWorldArea.WithY(-10), startWorldArea2.WithY(-10), endWorldArea.WithY(-10), endWorldArea2.WithY(-10),
                startWorldArea.WithY(10), startWorldArea2.WithY(10), endWorldArea.WithY(10), endWorldArea2.WithY(10)
            };
    }
}