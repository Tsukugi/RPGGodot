
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

public static class SelectionBase {
    public static List<NavigationCharacter> SelectActors(
        Vector3 startWorldArea,
        Vector3 endWorldArea,
        Array<Node> actorList) {

        Vector3 selectionWorldDistance =
            SelectionPanel.GetSize((Vector3)startWorldArea, (Vector3)endWorldArea);
        Vector3 selectionWorldStart =
            SelectionPanel.GetStartPosition((Vector3)startWorldArea, (Vector3)endWorldArea);

        /* We assume that every valid actor is a direct children of the player */
        List<NavigationCharacter> list = new();
        foreach (Node item in actorList) {
            if (item is NavigationCharacter actor) {
                bool isInArea = IsInArea(
                 actor.GlobalPosition,
                 selectionWorldStart,
                 selectionWorldStart + selectionWorldDistance);

                if (!isInArea) continue;
                list.Add(actor);
                GD.Print(actor.Name);
            }
        }

        return list;
    }

    public static bool IsInArea(Vector3 position, Vector3 start, Vector3 end) {
        return IsInAreaAxis(position.X, start.X, end.X)
            && IsInAreaAxis(position.Z, start.Z, end.Z);
    }

    public static bool IsInAreaAxis(float position, float start, float end) {
        return position >= start && position <= end;
    }
}