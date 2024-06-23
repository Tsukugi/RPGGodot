
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

public partial class RealTimeStrategyPlayer : PlayerBase {

    // Navigation
    float navigationGroupGapDistance = 1;
    public readonly NavigationInputHandler NavigationInputHandler = new();
    public readonly NavigationBase NavigationBase = new();

    // Selection
    List<NavigationUnit> selectedActors = new();
    List<NavigationUnit> actorsTargetedForSelection = new();
    SelectionPanel selectionPanel;
    Vector2 selectionAreaStart = Vector2.Zero;
    Vector2 selectionAreaEnd = Vector2.Zero;
    float minSelectionAreaForMultiSelection = 25;
    ShapeCast3D selectionShapeCast3D = new() {
        Position = VectorUtils.FarAway,
        Shape = new SphereShape3D() {
            Radius = 0.5f,
        },
        CollisionMask = (uint)CollisionMasks.Actor,
    };
    public List<NavigationUnit> SelectedActors {
        get => selectedActors;
        set {
            // TODO: Please improve me, i worry about this performance
            // Clear old and assign new selected state
            selectedActors.ForEach(actor => {
                actor.UnitSelection.IsSelected = false;
            });
            DebugLog("[SelectedActors.set] Cleared actors: " + selectedActors.Count);
            selectedActors = value;
            selectedActors.ForEach(actor => {
                actor.UnitSelection.IsSelected = true;
            });
            DebugLog("[SelectedActors.set] Added actors: " + selectedActors.Count);
        }
    }
    public ShapeCast3D SelectionShapeCast3D { get => selectionShapeCast3D; }

    // Camera
    Vector2 cameraDragStartPosition = Vector2.Zero;
    Vector2 cameraDragCurrentPosition = Vector2.Zero;


    public override void _Ready() {
        base._Ready();
        selectionPanel = GetNodeOrNull<SelectionPanel>(StaticNodePaths.PlayerUISelectionPanel);
        AddChild(selectionShapeCast3D);


        // !Debug
        return;
        if (!this.IsFirstPlayer()) {
            var children = GetChildren();

            Node areas = GetNodeOrNull("../NavigationRegion3D/Areas");
            var waypoints = areas.GetChildren();


            foreach (var child in children) {
                if (child is NavigationUnit unit) {
                    var shuffledWaypoints = waypoints;
                    waypoints.Shuffle();
                    Stack<Node3D> WayPoints = new();
                    foreach (var item in shuffledWaypoints) {
                        if (item is not Node3D waypoint) continue;
                        unit.UnitTask.Add(new UnitTaskMove(waypoint.GlobalPosition, unit));
                    }
                }
            }
        }
        // !EndDebug 
    }

    public override void _Input(InputEvent @event) {
        base._Input(@event);
        if (!this.IsFirstPlayer()) return;

        if (@event is InputEventMouseMotion) {
            if (Input.IsMouseButtonPressed(MouseButton.Left)) {
                selectionAreaEnd = Camera.GetViewport().GetMousePosition();
                selectionPanel.ApplySelectionTransform(selectionAreaStart, selectionAreaEnd);
                actorsTargetedForSelection = SelectActorsInArea();
            }

            if (Input.IsMouseButtonPressed(MouseButton.Middle)) {
                cameraDragCurrentPosition = Camera.GetViewport().GetMousePosition();
            }
        }

        if (@event is InputEventMouseButton eventMouseButton) {
            if (eventMouseButton.ButtonIndex == MouseButton.WheelDown) {
                Camera.Zoom(CameraZoomDirection.ZoomOut, 1);
            } else if (eventMouseButton.ButtonIndex == MouseButton.WheelUp) {
                Camera.Zoom(CameraZoomDirection.ZoomIn, 1);
            }
            if (eventMouseButton.ButtonIndex == MouseButton.Middle) {

                if (eventMouseButton.Pressed) {
                    DebugLog("[RealTimeStrategyPlayer._Input]: Start Camera Drag");
                    cameraDragStartPosition = Camera.GetViewport().GetMousePosition();
                } else {
                    DebugLog("[RealTimeStrategyPlayer._Input]: End Camera Drag");
                    // Reset
                    cameraDragStartPosition = Vector2.Zero;
                    cameraDragCurrentPosition = Vector2.Zero;
                }
            }


            if (eventMouseButton.ButtonIndex == MouseButton.Left) {
                // The Selecting phase is since the user presses the mouse button until he releases it
                if (eventMouseButton.Pressed) {
                    selectionAreaStart = Camera.GetViewport().GetMousePosition();
                    selectionAreaEnd = selectionAreaStart;
                    DebugLog("[RealTimeStrategyPlayer._Input]: Start Selection");
                } else {
                    float selectionAreaSize = VectorUtils.GetDistanceFromVectors(selectionAreaStart, selectionAreaEnd);
                    bool isSingleSelection = selectionAreaSize < minSelectionAreaForMultiSelection;
                    DebugLog("[RealTimeStrategyPlayer._Input]: selectionAreaSize " + selectionAreaSize);

                    if (isSingleSelection) {
                        Vector3? worldSelectionPoint = NavigationBase.Get3DWorldPosition(Camera, selectionAreaStart);
                        if (worldSelectionPoint is not Vector3 foundWorldSelectionPoint) return;
                        selectionShapeCast3D.GlobalPosition = foundWorldSelectionPoint;
                        SelectedActors = new List<NavigationUnit>();
                        DebugLog("[RealTimeStrategyPlayer._Input]: SelectedActorsCleared");
                    } else {
                        SelectedActors = actorsTargetedForSelection;
                        DebugLog("[RealTimeStrategyPlayer._Input]: SelectedActorsAdded Count: " + SelectedActors.Count);
                    }
                    // Reset
                    selectionAreaEnd = Vector2.Zero;
                    selectionAreaStart = Vector2.Zero;
                    selectionPanel.ResetPosition();
                    DebugLog("[RealTimeStrategyPlayer._Input]: Selection Finished");
                }
            }
            if (eventMouseButton.ButtonIndex == MouseButton.Right) {
                if (eventMouseButton.Pressed) {
                    Vector3? targetPosition = NavigationBase.GetNavigationTargetPosition(Camera);
                    if (targetPosition is not Vector3 targetPositionInWorld) return;
                    SelectionBase.ApplyCommandToGroupPosition(
                        SelectedActors,
                        targetPositionInWorld,
                        navigationGroupGapDistance,
                        (float)System.Math.Floor(System.Math.Sqrt(SelectedActors.Count)),
                        ApplyNavigation);
                }
            }
        }
    }

    static void ApplyNavigation(NavigationUnit unit, Vector3 targetPosition) {
        if (unit.UnitSelection.IsSelected) {
            unit.NavigationAgent.StartNewNavigation(targetPosition);
        } else {
            unit.UnitTask.Add(new UnitTaskMove(targetPosition, unit));
        }
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!SimpleGameManager.IsFirstPlayer(this)) return;

        // Camera WASD move
        Vector2 axis = NavigationInputHandler.GetAxis();
        if (axis != Vector2.Zero) {
            Camera.AxisMove(axis, (float)delta);
        }

        // Camera Middle mouse button drag move
        if (cameraDragCurrentPosition != Vector2.Zero) {
            Camera.AxisMove((cameraDragCurrentPosition - cameraDragStartPosition).Normalized(), (float)delta);
        }

        // Single Selection 
        Array collisions = selectionShapeCast3D.CollisionResult;
        if (collisions.Count > 0) {
            SelectionBase.SelectActor(this, collisions);
        }
    }

    List<NavigationUnit> SelectActorsInArea() {
        Vector3? startWorldArea = NavigationBase.Get3DWorldPosition(Camera, selectionAreaStart);
        Vector3? endWorldArea = NavigationBase.Get3DWorldPosition(Camera, selectionAreaEnd);
        if (startWorldArea is not Vector3 foundStartWorldArea
            || endWorldArea is not Vector3 foundEndWorldArea) {
            return new List<NavigationUnit>();
        }
        return SelectionBase.SelectActors(foundStartWorldArea, foundEndWorldArea, GetChildren());
    }


    static bool IsNavigationUnit(Node node) {
        return node is NavigationUnit;
    }

    public List<NavigationUnit> GetAllUnits() {
        return (List<NavigationUnit>)GetChildren().ToList().Where(IsNavigationUnit);
    }
}