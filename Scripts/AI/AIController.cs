using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class AIController : Node {

    Stack<Node3D> wayPoints = new();
    Timer behaviourCheckTimer = new() {
        OneShot = false,
        WaitTime = 1f + new Random().NextDouble(),
    };
    NavigationUnit unit;

    float waypointDistanceSafeRadius = 3f;
    bool recycleWaypoints = true;

    public Stack<Node3D> WayPoints { get => wayPoints; set => wayPoints = value; }

    public override void _Ready() {
        base._Ready();
        // ! We assume that we use this as a child of a NavigationUnit
        unit = GetParentOrNull<NavigationUnit>();

        AddChild(behaviourCheckTimer);
        behaviourCheckTimer.Timeout += OnBehaviourCheck;
        behaviourCheckTimer.Start();

    }

    void OnBehaviourCheck() {
        if (wayPoints.Count > 0) {
            Vector3 nextPosition = wayPoints.First().GlobalPosition;
            float distance = VectorUtils.GetDistanceFromVectors(unit.GlobalPosition, nextPosition);
            if (unit.NavigationTargetPosition != nextPosition) {
                unit.NavigationTargetPosition = nextPosition;
                GD.Print("[OnBehaviourCheck] Set next WayPoint to " + unit.NavigationTargetPosition);
            } else if (distance < waypointDistanceSafeRadius) {
                if (!unit.IsMoving) {
                    unit.NavigationTargetPosition = unit.GlobalPosition;
                }
                Node3D usedPoint = wayPoints.Pop();
                GD.Print("[OnBehaviourCheck] Waypoint Reached");
                if (recycleWaypoints) {
                    wayPoints.Append(usedPoint);
                }
                if (wayPoints.Count == 0) {
                    GD.Print("[OnBehaviourCheck] Final destination reached");
                }
            } else {
                // Is moving
            }
        }
    }
}