
using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

public static class SelectionBase {

    public static List<NavigationUnit> SelectActor(
       Vector3 worldPointPosition) {

        // TODO: Implement me
        List<NavigationUnit> list = new();

        return list;
    }
    public static List<NavigationUnit> SelectActors(
        Vector3 startWorldArea,
        Vector3 endWorldArea,
        Array<Node> actorList) {

        Vector3 selectionWorldDistance =
            SelectionPanel.GetSize(startWorldArea, endWorldArea);
        Vector3 selectionWorldStart =
            SelectionPanel.GetStartPosition(startWorldArea, endWorldArea);

        /* We assume that every valid actor is a direct children of the player */
        List<NavigationUnit> list = new();
        foreach (Node item in actorList) {
            if (item is NavigationUnit actor) {
                bool isInArea = IsInArea(
                 actor.GlobalPosition,
                 selectionWorldStart,
                 selectionWorldStart + selectionWorldDistance);

                if (!isInArea) continue;
                list.Add(actor);
            }
        }

        return list;
    }

    public static void ApplyGroupPosition(List<NavigationUnit> group, Vector3 targetPosition, float gapDistance, float rowLimit) {
        for (int i = 0; i < group.Count; i++) {
            if (!SimpleGameManager.IsFirstPlayerControlled(group[i].Player)) continue;
            group[i].NavigationTargetPosition = new Vector3(
                targetPosition.X + i * gapDistance % rowLimit,
                targetPosition.Y,
                targetPosition.Z + i * (gapDistance / rowLimit));
        }
    }


    public static bool IsInArea(Vector3 position, Vector3 start, Vector3 end) {
        return IsInAreaAxis(position.X, start.X, end.X)
            && IsInAreaAxis(position.Z, start.Z, end.Z);
    }

    public static bool IsInAreaAxis(float position, float start, float end) {
        return position >= start && position <= end;
    }
}

public enum SelectionPhase {
    Idle,
    SingleSelection,
    MultipleSelection
}