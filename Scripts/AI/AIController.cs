using System;
using System.Collections.Generic;
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
        unit = this.TryFindParentNodeOfType<NavigationUnit>();

        AddChild(behaviourCheckTimer);
        behaviourCheckTimer.Timeout += OnBehaviourCheck;
        behaviourCheckTimer.Start();
    }

    void OnBehaviourCheck() {
        // TODO Implement me
    }
}