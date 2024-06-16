
using System;
using System.Collections.Generic;
using Godot;

public partial class RealTimeStrategyPlayer : PlayerBase {

    public readonly NavigationInputHandler NavigationInputHandler = new();
    public readonly NavigationBase NavigationBase = new();
    private List<NavigationCharacter> selectedActors = new();
    private List<NavigationCharacter> actorsTargetedForSelection = new();
    private Color selectionBoxColor = new(0, 1, 0, 0.3f);
    private bool isSelecting = false;
    private SelectionPanel selectionPanel;
    private Vector2 selectionAreaStart = Vector2.Zero;
    private Vector2 selectionAreaEnd = Vector2.Zero;

    private int navigationGroupRowLimit = 5;
    private int navigationGroupGapDistance = 1;

    public List<NavigationCharacter> SelectedActors {
        get => selectedActors;
        set {
            // TODO: Please improve me, i worry about this performance
            // Clear old and assign new selected state
            selectedActors.ForEach(actor => {
                actor.isSelected = false;
            });
            value.ForEach(actor => {
                actor.isSelected = true;
            });
            selectedActors = value;
        }
    }

    public override void _Ready() {
        base._Ready();
        selectionPanel = GetNode<SelectionPanel>(Constants.PlayerUISelectionPanelPath);

    }
    public override void _Input(InputEvent @event) {
        base._Input(@event);
        if (!SimpleGameManager.IsFirstPlayerControlled(this)) return;

        if (@event is InputEventMouseMotion && isSelecting) {
            selectionAreaEnd = Camera.GetViewport().GetMousePosition();
            selectionPanel.ApplySelectionTransform(selectionAreaStart, selectionAreaEnd);
            actorsTargetedForSelection = SelectActors();
        }

        if (@event is InputEventMouseButton eventMouseButton) {
            if (eventMouseButton.ButtonIndex == MouseButton.WheelDown) {
                Camera.Zoom(CameraZoomDirection.ZoomOut, 1);
            } else if (eventMouseButton.ButtonIndex == MouseButton.WheelUp) {
                Camera.Zoom(CameraZoomDirection.ZoomIn, 1);
            }

            if (eventMouseButton.ButtonIndex == MouseButton.Left) {
                isSelecting = eventMouseButton.Pressed;
                if (eventMouseButton.Pressed) {
                    selectionAreaEnd = Camera.GetViewport().GetMousePosition();
                    selectionAreaStart = Camera.GetViewport().GetMousePosition();
                    GD.Print("[RealTimeStrategyPlayer._Input]: Start Selection");
                } else {
                    SelectedActors = actorsTargetedForSelection;
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

                    NavigationInputHandler.worldMouseNavigationTargetCoordinates = targetPositionInWorld;
                    SelectionBase.ApplyGroupPosition(SelectedActors, targetPositionInWorld, navigationGroupGapDistance, navigationGroupRowLimit);
                }
            }
        }
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        Vector2 axis = NavigationInputHandler.GetAxis();
        Camera.AxisMove(axis, (float)delta);
        GD.Print("TimeFps: " + Performance.GetMonitor(Performance.Monitor.TimeFps)); 
        GD.Print("RenderTotalObjectsInFrame: " + Performance.GetMonitor(Performance.Monitor.RenderTotalObjectsInFrame)); 
        GD.Print("NavigationAgentCount: " + Performance.GetMonitor(Performance.Monitor.NavigationAgentCount)); 
        GD.Print("TimeNavigationProcess: " + Performance.GetMonitor(Performance.Monitor.TimeNavigationProcess)); 
        GD.Print("TimeProcess: " + Performance.GetMonitor(Performance.Monitor.TimeProcess));  
        GD.Print("RenderVideoMemUsed: " + Performance.GetMonitor(Performance.Monitor.RenderVideoMemUsed)); 
    }


    private List<NavigationCharacter> SelectActors() {
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