using System;
using System.Linq;
using Godot;

public partial class CameraBase : Camera3D {

    [Export]
    bool findAnyActorOnStartEnabled = false;
    [Export]
    Unit selectedActor = null;
    [Export]
    float cameraVelocity = 3;
    [Export]
    float cameraZoomVelocity = 0.5f;
    public event EventHandler<Vector3> OnRotationChange;
    // Offsets 
    public static readonly Vector3 CameraTransformOffset = new(-8, 8, 8);
    public static readonly Vector3 CameraRotationOffset = new(-45, -45, 0);
    public static readonly int CameraRotationAxisOffset = 45;
    Vector3 currentTransformOffset = CameraTransformOffset;
    Vector3 currentRotationOffset = CameraRotationOffset;
    ProjectionType projectionType = ProjectionType.Perspective;

    /* public static readonly Vector3 CameraTransformOffset = new(-8, 7, 8);
     public static readonly Vector3 CameraRotationOffset = new(-30, -45, 0); 
     ProjectionType projectionType = ProjectionType.Orthogonal;*/
    int cameraOrthogonalSize = 10;

    int edgeMovementPadding = 50;

    public Unit SelectedActor {
        get => selectedActor;
        set {
            selectedActor = value;
            AttachToActor(value);
        }
    }


    public override void _Ready() {
        base._Ready();
        PlayerBase player = this.TryFindParentNodeOfType<PlayerBase>();
        Current = player.IsFirstPlayer();
        
        UpdateCameraProperties();
        if (findAnyActorOnStartEnabled) {
            Unit newActor = player.GetChildren().OfType<Unit>().FirstOrDefault();
            if (newActor == null) return;
            SelectedActor = newActor;
        }
    }

    public override void _Process(double delta) {
        base._Process(delta);
        if (SelectedActor != null) FollowActor();
    }

    void FollowActor() {
        try {
            Vector3 offset = currentTransformOffset + SelectedActor.Transform.Origin;
            Vector3 rotation = currentRotationOffset + SelectedActor.RotationDegrees;
            // Follow the selected actor
            Reposition(offset, rotation);
        } catch (ObjectDisposedException) {
            // We will reset camera if the selectedActor was removed in the meanwhile.
            SelectedActor = null;
        }
    }

    public Vector2 GetEdgeMovingDirection() {
        Vector2 mousePosition = GetViewport().GetMousePosition();
        Vector2 viewportSize = GetViewport().GetVisibleRect().Size;
        Rect2 paddedArea = new(
            new Vector2(edgeMovementPadding, edgeMovementPadding),
            new Vector2(
                viewportSize.X - edgeMovementPadding,
                viewportSize.Y - edgeMovementPadding));

        Vector2 cameraEdgeMovingPosition = Vector2.Zero;
        if (mousePosition.X < paddedArea.Position.X) cameraEdgeMovingPosition.X = -1;
        else if (mousePosition.X > paddedArea.Size.X) cameraEdgeMovingPosition.X = 1;
        if (mousePosition.Y < paddedArea.Position.Y) cameraEdgeMovingPosition.Y = -1;
        else if (mousePosition.Y > paddedArea.Size.Y) cameraEdgeMovingPosition.Y = 1;
        return cameraEdgeMovingPosition.Rotate(CameraRotationAxisOffset);
    }

    static Rect2 GetBoundariesRect(int size) {
        int halfSize = size / 2;
        Rect2 boundaries = new(new Vector2(-halfSize, -halfSize), new Vector2(size, size));
        return boundaries;
    }

    public void AxisMove(Vector2 axis, float delta) {
        if (axis == Vector2.Zero) return;
        if (SelectedActor != null) SelectedActor = null;

        Position = new Vector3(
            Position.X + axis.X * cameraVelocity * delta,
            Position.Y,
            Position.Z + axis.Y * cameraVelocity * delta);
    }


    public void Zoom(CameraZoomDirection value, float delta) {
        if (value == 0) return;
        Vector3 newOffset = new(
            Position.X,
            Position.Y + (float)value * cameraZoomVelocity * delta,
            Position.Z);

        Position = newOffset;
        currentTransformOffset.Y = newOffset.Y;
    }

    public void AttachToActor(Unit actor) {
        if (actor == null) {
            GD.Print("[CameraBase.AttachToActor] There is not a Unit available to attach");
        } else {
            GD.Print("[CameraBase.AttachToActor] Attaching camera to " + SelectedActor.Name);
            UpdateCameraProperties();
        }
    }

    public void Reposition(Vector3 offset, Vector3 rotation) {
        Position = offset;
        RotationDegrees = rotation;
    }

    public void UpdateCameraProperties() {
        Reposition(currentTransformOffset, currentRotationOffset);
        Projection = projectionType;
        if (Projection == ProjectionType.Orthogonal) {
            Size = cameraOrthogonalSize;
        }
    }

}
