using System.Collections.Generic;
using Godot;
using Godot.Collections;

public static class SelectionUtils {
    /// We use this function to extract the Units that come from a collision. 
    public static Unit GetColliderUnit(this Variant item) {
        // We know that item.collider is a Unit but can be a Unit 
        return item.AsGodotDictionary()["collider"].As<Unit>();
    }

    public static void SelectActors(
        System.Action<List<Unit>> OnSelection, Array collisions) {
        List<Unit> actors = new();
        foreach (Variant item in collisions) {
            if (item.GetColliderUnit() is not Unit unit) continue;
            actors.Add(unit);
        }
        OnSelection(actors);
    }

    public static void SelectActors(
           System.Action<List<Unit>> OnSelection, Array collisions, string? playerControllerName = null) {
        List<Unit> units = new();
        List<Unit> playerUnits = new();
        List<Unit> firstPickedUnitPlayerUnits = new();
        string pickedPlayerName = null;
        foreach (Variant item in collisions) {
            if (item.GetColliderUnit() is not Unit unit) continue;
            if (pickedPlayerName is null) pickedPlayerName = unit.Player.Name;

            // Add units that are NavUnits
            units.Add(unit);
            // Add units that are playerControllerName's
            if (playerControllerName is string playerName && unit.Player.Name == playerName) {
                playerUnits.Add(unit);
            }
            // Add units that are the same player as the first picked unit
            if (unit.Player.Name == pickedPlayerName) {
                firstPickedUnitPlayerUnits.Add(unit);
            }
        }

        // If we have units that are the Controller Player's we use them, 
        // if not we will use the units that are the same player as the first picked one. To avoid selecting every NavUnit from different players
        // Else we use all units.
        if (playerUnits.Count > 0) OnSelection(playerUnits);
        else if (firstPickedUnitPlayerUnits.Count > 0) OnSelection(firstPickedUnitPlayerUnits);
        else OnSelection(units);
    }


    public static List<Unit> SelectActorsInList(
    Vector3 startWorldArea,
    Vector3 endWorldArea,
    Array<Node> actorList) {

        Vector3 selectionWorldDistance =
            SelectionPanel.GetSize(startWorldArea, endWorldArea);
        Vector3 selectionWorldStart =
            SelectionPanel.GetStartPosition(startWorldArea, endWorldArea);

        /* We assume that every valid actor is a direct children of the player */
        List<Unit> list = new();
        foreach (Node item in actorList) {
            if (item is Unit actor) {
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

    public static void ApplyCommandToGroupPosition(List<Unit> group, Vector3 targetPosition, float gapDistance, float rowLimit, System.Action<Unit, Vector3> command) {
        for (int i = 0; i < group.Count; i++) {
            if (!group[i].Player.IsFirstPlayer()) continue;
            Vector3 navigationTarget = new(
                    targetPosition.X + i * gapDistance % rowLimit,
                    targetPosition.Y,
                    targetPosition.Z + i * (gapDistance / rowLimit));
            command(group[i], navigationTarget);
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