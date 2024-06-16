using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

public partial class RealTimeStrategyPlayer : PlayerBase {

    public readonly NavigationInputHandler NavigationInputHandler = new();
    public readonly NavigationBase NavigationBase = new();
    private List<Node> SelectedPlayers;
    private Color selectionBoxColor = new(0, 1, 0, 0.3f);
    private bool isSelecting = false;
    private SelectionPanel selectionPanel;
    private Vector2 selectionPositionStart = Vector2.Zero;
    private Vector2 selectionPositionEnd = Vector2.Zero;

    private BoxShape3D selectionBoxShape3D = new() {
        Size = Vector3.Zero
    };
    private PhysicsShapeQueryParameters3D selectionQuery;
    public override void _Ready() {
        base._Ready();
        selectionPanel = GetNode<SelectionPanel>(Constants.PlayerUISelectionPanelPath);
        selectionQuery = new() {
            Transform = new Transform3D(new Basis(), Vector3.Zero),
            Shape = selectionBoxShape3D,
            CollisionMask = (uint)CollisionMasks.Actor,
        };
    }
    public override void _Input(InputEvent @event) {
        base._Input(@event);
        if (!SimpleGameManager.IsFirstPlayerControlled(this)) return;


        if (@event is InputEventMouseMotion && isSelecting) {
            selectionPositionEnd = Camera.GetViewport().GetMousePosition();
            selectionPanel.ApplySelectionTransform(selectionPositionStart, selectionPositionEnd);
        }

        if (@event is InputEventMouseButton eventMouseButton) {
            if (eventMouseButton.ButtonIndex == MouseButton.Left) {
                isSelecting = eventMouseButton.Pressed;
                if (eventMouseButton.Pressed) {
                    selectionPositionEnd = Camera.GetViewport().GetMousePosition();
                    selectionPositionStart = Camera.GetViewport().GetMousePosition();
                    GD.Print("Start Selection");
                } else {
                    select();
                    // Reset
                    selectionPositionEnd = Vector2.Zero;
                    selectionPositionStart = Vector2.Zero;
                    selectionPanel.ResetPosition();
                    GD.Print("Selection Finished");
                }
            }
            if (eventMouseButton.ButtonIndex == MouseButton.Right) {

            }
        }
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        Vector2 axis = NavigationInputHandler.GetAxis();
        Camera.AxisMove(axis, (float)delta);

        if (isSelecting) {

        }
    }

    private static Vector3 Vector2To3(Vector2 direction) {
        return new Vector3(direction.X, 10, direction.Y);
    }

    private void select() {
        PhysicsDirectSpaceState3D space = Camera.GetWorld3D().DirectSpaceState;

        Vector3? startWorldPosition = NavigationBase.Get3DWorldPosition(Camera, selectionPositionStart);
        Vector3? endWorldPosition = NavigationBase.Get3DWorldPosition(Camera, selectionPositionEnd);

        if (startWorldPosition == null || endWorldPosition == null) return;
        Vector3 selectionWorldDistance =
            SelectionPanel.GetSize((Vector3)startWorldPosition, (Vector3)endWorldPosition);
        Vector3 selectionWorldStartPosition =
            SelectionPanel.GetStartPosition((Vector3)startWorldPosition, (Vector3)endWorldPosition);
        Vector3 selectionWorldCenter =
            selectionWorldStartPosition + (selectionWorldDistance / 2);
        selectionWorldCenter.Y = 0.5f;

        selectionBoxShape3D.Size = new Vector3(
            selectionWorldDistance.X,
            1,
            selectionWorldDistance.Z
        );
        selectionQuery.Transform = new Transform3D(new Basis(), selectionWorldCenter);
        Array<Dictionary> selected = space.IntersectShape(selectionQuery);


        var startBox = GetNode<MeshInstance3D>("StartBox");
        startBox.Scale = ((BoxShape3D)selectionQuery.Shape).Size;
        startBox.Position = selectionQuery.Transform.Origin;

        if (selected.Count == 0) return;


        foreach (var item in selected) {
            GD.Print(item["collider"].As<NavigationCharacter>().Name);
        }
    }
}