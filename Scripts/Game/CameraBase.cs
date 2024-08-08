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
    // Offsets 
    [Export]
    float transformX = -8;
    [Export]
    float transformY = 8;
    [Export]
    float transformZ = 8;
    [Export]
    float rotationX = -45;
    [Export]
    float rotationY = -45;
    [Export]
    float rotationZ = 45;

    public Vector3 currentTransformOffset {
        get => new(transformX, transformY, transformZ);
        set {
            transformX = value.X;
            transformY = value.Y;
            transformZ = value.Z;
            Position = value;
        }
    }
    public Vector3 currentRotationOffset {
        get => new(rotationX, rotationY, rotationZ);
        set {
            rotationX = value.X;
            rotationY = value.Y;
            rotationZ = value.Z;
            RotationDegrees = value;
        }
    }
    public static readonly int CameraRotationAxisOffset = 45;
    public event EventHandler<Vector3> OnRotationChange;

    [Export]
    ProjectionType projectionType = ProjectionType.Orthogonal;

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

        switch (projectionType) {
            case ProjectionType.Perspective: break;
            case ProjectionType.Orthogonal: {
                    cameraVelocity /= 10f;
                    currentTransformOffset += currentTransformOffset.AddToY(4).Magnitude(7);
                    Reposition(currentTransformOffset, currentRotationOffset);
                    break;
                }
        }

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
        switch (projectionType) {
            case ProjectionType.Perspective: PerspectiveZoom(value, delta); return;
            case ProjectionType.Orthogonal: OrthographicZoom(value, delta); return;
        }
    }

    void OrthographicZoom(CameraZoomDirection value, float delta) {
        Size += (float)value * cameraZoomVelocity * delta;
    }

    void PerspectiveZoom(CameraZoomDirection value, float delta) {
        Vector3 newOffset = new(
            Position.X,
            Position.Y + (float)value * cameraZoomVelocity * delta,
            Position.Z);

        Position = newOffset;
        currentTransformOffset = currentTransformOffset.WithY(newOffset.Y);
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
