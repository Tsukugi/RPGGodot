using System;
using System.Linq;
using Godot;

public partial class CameraBase : Camera3D {

    [Export]
    ActorBase selectedActor = null;
    [Export]
    float cameraVelocity = 3;
    [Export]
    float cameraZoomVelocity = 0.5f;
    public event EventHandler<Vector3> OnRotationChange;

    // Offsets 

    public static readonly Vector3 CameraTransformOffset = new(0, 8, 0);
    public static readonly Vector3 CameraRotationOffset = new(-90, 0, 0);
    ProjectionType projectionType = ProjectionType.Perspective;

    /* public static readonly Vector3 CameraTransformOffset = new(-8, 7, 8);
     public static readonly Vector3 CameraRotationOffset = new(-30, -45, 0); 
     ProjectionType projectionType = ProjectionType.Orthogonal;*/
    int cameraOrthogonalSize = 10;

    public ActorBase SelectedActor {
        get => selectedActor;
        set {
            selectedActor = value;
            AttachToActor(value);
        }
    }


    public override void _Ready() {
        base._Ready();
        Node parent = GetParent();
        if (parent is not PlayerBase player) throw new Exception("[CameraBase._Ready] Parent is not player, it is" + parent);

        Current = player.IsFirstPlayer();

        ActorBase newActor = parent.GetChildren().OfType<ActorBase>().FirstOrDefault();
        if (newActor == null) return;
        SelectedActor = newActor;
    }

    public override void _Process(double delta) {
        base._Process(delta);
        if (SelectedActor == null) return;
        try {
            // Follow the selected actor
            Reposition(CameraTransformOffset + SelectedActor.Transform.Origin, CameraRotationOffset + SelectedActor.RotationDegrees);
        } catch (ObjectDisposedException) {
            // We will reset camera if the selectedActor was removed in the meanwhile.
            SelectedActor = null;
        }
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
        //CameraBase.CameraRotationOffset.Y = newOffset.Y;
    }

    public void AttachToActor(ActorBase actor) {
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
        Reposition(CameraTransformOffset, CameraRotationOffset);
        Projection = projectionType;
        if (Projection == ProjectionType.Orthogonal) {
            Size = cameraOrthogonalSize;
        }
    }

    internal void AxisMove(CameraZoomDirection zoomIn, int v) {
        throw new NotImplementedException();
    }
}
