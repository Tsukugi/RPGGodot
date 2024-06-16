using System;
using System.Linq;
using Godot;

public partial class CameraBase : Camera3D {

    [Export]
    private ActorBase selectedActor = null;
    [Export]
    private float cameraVelocity = 3;
    public event EventHandler<Vector3> OnRotationChange;

    // Offsets 

    private Vector3 cameraTransformOffset = new(0, 8, 0);
    private Vector3 cameraRotationOffset = new(-90, 0, 0);
    private ProjectionType projectionType = ProjectionType.Perspective;


    /*
    private Vector3 cameraTransformOffset = new(8, 7, 8);
    private Vector3 cameraRotationOffset = new(-30, 45, 0);
    private ProjectionType projectionType = ProjectionType.Orthogonal;
    */
    private int cameraOrthogonalSize = 10;


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
        Current = SimpleGameManager.IsFirstPlayerControlled(parent);

        ActorBase newActor = parent.GetChildren().OfType<ActorBase>().FirstOrDefault();
        if (newActor == null) return;
        SelectedActor = newActor;
    }

    public override void _Process(double delta) {
        base._Process(delta);
        if (SelectedActor == null) return;
        try {
            // Follow the selected actor
            Reposition(cameraTransformOffset + SelectedActor.Transform.Origin, cameraRotationOffset + SelectedActor.RotationDegrees);
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

    public void AttachToActor(ActorBase actor) {
        if (actor == null) {
            GD.Print("[CameraBase.AttachToActor] There is not a Character available to attach");
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
        Reposition(cameraTransformOffset, cameraRotationOffset);
        Projection = projectionType;
        if (Projection == ProjectionType.Orthogonal) {
            Size = cameraOrthogonalSize;
        }
    }

}
