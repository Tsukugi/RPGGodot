
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

public partial class RealTimeStrategyPlayer : PlayerBase {

    // Navigation
    private int navigationGroupRowLimit = 5;
    private float navigationGroupGapDistance = 1;
    public readonly NavigationInputHandler NavigationInputHandler = new();
    public readonly NavigationBase NavigationBase = new();

    // Selection
    private List<NavigationCharacter> selectedActors = new();
    private List<NavigationCharacter> actorsTargetedForSelection = new();
    private SelectionPanel selectionPanel;
    private Vector2 selectionAreaStart = Vector2.Zero;
    private Vector2 selectionAreaEnd = Vector2.Zero;
    private float minSelectionAreaForMultiSelection = 25;
    private ShapeCast3D selectionShapeCast3D = new() {
        Position = VectorUtils.FarAway,
        Shape = new SphereShape3D() {
            Radius = 0.5f,
        },
        CollisionMask = (uint)CollisionMasks.Actor,
    };
    public List<NavigationCharacter> SelectedActors {
        get => selectedActors;
        set {
            // TODO: Please improve me, i worry about this performance
            // Clear old and assign new selected state
            selectedActors.ForEach(actor => {
                actor.IsSelected = false;
            });
            GD.Print("[SelectedActors.set] Cleared actors: " + selectedActors.Count);
            selectedActors = value;
            selectedActors.ForEach(actor => {
                actor.IsSelected = true;
            });
            GD.Print("[SelectedActors.set] Added actors: " + selectedActors.Count);
        }
    }


    // Camera
    private Vector2 cameraDragStartPosition = Vector2.Zero;
    private Vector2 cameraDragCurrentPosition = Vector2.Zero;


    public override void _Ready() {
        base._Ready();
        selectionPanel = GetNode<SelectionPanel>(Constants.PlayerUISelectionPanelPath);
        AddChild(selectionShapeCast3D);
    }

    public override void _Input(InputEvent @event) {
        base._Input(@event);
        if (!SimpleGameManager.IsFirstPlayerControlled(this)) return;

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
                    GD.Print("[RealTimeStrategyPlayer._Input]: Start Camera Drag");
                    cameraDragStartPosition = Camera.GetViewport().GetMousePosition();
                } else {
                    GD.Print("[RealTimeStrategyPlayer._Input]: End Camera Drag");
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
                    GD.Print("[RealTimeStrategyPlayer._Input]: Start Selection");
                } else {
                    float selectionAreaSize = VectorUtils.GetDistanceFromVectors(selectionAreaStart, selectionAreaEnd);
                    bool isSingleSelection = selectionAreaSize < minSelectionAreaForMultiSelection;
                    GD.Print("[RealTimeStrategyPlayer._Input]: selectionAreaSize " + selectionAreaSize);

                    if (isSingleSelection) {
                        Vector3? worldSelectionPoint = NavigationBase.Get3DWorldPosition(Camera, selectionAreaStart);
                        if (worldSelectionPoint is not Vector3 foundWorldSelectionPoint) return;
                        selectionShapeCast3D.GlobalPosition = foundWorldSelectionPoint;
                        SelectedActors = new List<NavigationCharacter>();
                        GD.Print("[RealTimeStrategyPlayer._Input]: SelectedActorsCleared");
                    } else {
                        SelectedActors = actorsTargetedForSelection;
                        GD.Print("[RealTimeStrategyPlayer._Input]: SelectedActorsAdded Count: " + SelectedActors.Count);
                    }
                    // Reset
                    selectionAreaEnd = Vector2.Zero;
                    selectionAreaStart = Vector2.Zero;
                    selectionPanel.ResetPosition();
                    GD.Print("[RealTimeStrategyPlayer._Input]: Selection Finished");
                }
            }
            if (eventMouseButton.ButtonIndex == MouseButton.Right) {
                if (eventMouseButton.Pressed) {
                    Vector3? targetPosition = NavigationBase.GetNavigationTargetPosition(Camera);
                    if (targetPosition is not Vector3 targetPositionInWorld) return;
                    SelectionBase.ApplyGroupPosition(
                        SelectedActors,
                        targetPositionInWorld,
                        navigationGroupGapDistance,
                        (float)System.Math.Floor(System.Math.Sqrt(SelectedActors.Count)));
                }
            }
        }
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        Vector2 axis = NavigationInputHandler.GetAxis();
        if (axis != Vector2.Zero) {
            Camera.AxisMove(axis, (float)delta);
        }

        if (cameraDragCurrentPosition != Vector2.Zero) {
            Camera.AxisMove((cameraDragCurrentPosition - cameraDragStartPosition).Normalized(), (float)delta);
        }


        // Single Selection 
        Array collisions = selectionShapeCast3D.CollisionResult;
        if (collisions.Count > 0) {
            actorsTargetedForSelection.Clear();
            foreach (Variant item in collisions) {
                // We know that item.collider is a NavigationCharacter 
                var collider = item.AsGodotDictionary()["collider"].As<NavigationCharacter>();
                if (collider is NavigationCharacter navigationCharacter) {
                    actorsTargetedForSelection.Add(navigationCharacter);
                }
            }
            SelectedActors = actorsTargetedForSelection;
            selectionShapeCast3D.GlobalPosition = VectorUtils.FarAway;
        }

        /*
                GD.Print("TimeFps: " + Performance.GetMonitor(Performance.Monitor.TimeFps));
                GD.Print("RenderTotalObjectsInFrame: " + Performance.GetMonitor(Performance.Monitor.RenderTotalObjectsInFrame));
                GD.Print("NavigationAgentCount: " + Performance.GetMonitor(Performance.Monitor.NavigationAgentCount));
                GD.Print("TimeNavigationProcess: " + Performance.GetMonitor(Performance.Monitor.TimeNavigationProcess));
                GD.Print("TimeProcess: " + Performance.GetMonitor(Performance.Monitor.TimeProcess));
                GD.Print("RenderVideoMemUsed: " + Performance.GetMonitor(Performance.Monitor.RenderVideoMemUsed));
        */
    }

    private List<NavigationCharacter> SelectActorsInArea() {
        Vector3? startWorldArea = NavigationBase.Get3DWorldPosition(Camera, selectionAreaStart);
        Vector3? endWorldArea = NavigationBase.Get3DWorldPosition(Camera, selectionAreaEnd);
        if (startWorldArea is not Vector3 foundStartWorldArea
            || endWorldArea is not Vector3 foundEndWorldArea) {
            return new List<NavigationCharacter>();
        }
        return SelectionBase.SelectActors(foundStartWorldArea, foundEndWorldArea, GetChildren());
    }

    private static Vector3 Vector2To3(Vector2 direction) {
        return new Vector3(direction.X, 10, direction.Y);
    }

}