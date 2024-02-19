using System;
using System.Linq;
using Godot;

public partial class CameraBase : Camera3D {

    [Export]
    private ActorBase selectedActor = null;
    public event EventHandler<Vector3> OnRotationChange;

    // Offsets
    private Vector3 cameraTransformOffset = new(8, 7, 8);
    private Vector3 cameraRotationOffset = new(-30, 45, 0);
    private ProjectionType projectionType = ProjectionType.Orthogonal;
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
        GD.Print(parent.Name);
        Current = SimpleGameManager.IsFirstPlayerControlled(parent);

        ActorBase newActor = parent.GetChildren().OfType<ActorBase>().FirstOrDefault();
        if (newActor == null) return;
        SelectedActor = newActor;
    }

    public override void _Process(double delta) {
        base._Process(delta);
        if (SelectedActor == null) return;
        // Follow the selected actor
        Reposition(cameraTransformOffset + SelectedActor.Transform.Origin, cameraRotationOffset + SelectedActor.RotationDegrees);
    }


    public void AttachToActor(ActorBase actor) {
        if (actor == null) {
            GD.Print("[CameraBase.AttachToActor] Selected Player not found, not assigning camera");
        } else {
            GD.Print("[CameraBase.AttachToActor] Assigning camera to " + SelectedActor.Name);
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
