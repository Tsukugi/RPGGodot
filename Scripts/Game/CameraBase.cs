using System;
using Godot;

public partial class CameraBase : Camera3D {

    [Export]
    private ActorBase selectedPlayerActor = null;
    public event EventHandler<Vector3> OnRotationChange;

    // Offsets
    private Vector3 cameraTransformOffset = new(8, 7, 8);
    private Vector3 cameraRotationOffset = new(-30, 45, 0);
    private ProjectionType projectionType = ProjectionType.Orthogonal;
    private int cameraOrthogonalSize = 10;

    public ActorBase SelectedPlayerActor {
        get => selectedPlayerActor;
        set {
            selectedPlayerActor = value;
            AttachToActor(value);
        }
    }


    public override void _Ready() {
        AttachToActor(selectedPlayerActor);
    }

    public void AttachToActor(ActorBase actor) {
        if (actor == null) {
            GD.Print("[CameraBase.AttachToActor] Selected Player not found, not assigning camera");
        } else {
            GD.Print("[CameraBase.AttachToActor] Assigning camera to " + selectedPlayerActor.Name);

            CallDeferred("reparent", actor, true);
            Reposition(cameraTransformOffset, cameraRotationOffset);
        }
    }

    public void Reposition(Vector3 offset, Vector3 rotation) {
        Position = offset;
        RotationDegrees = rotation;
        Projection = projectionType;
        if (Projection == ProjectionType.Orthogonal) {
            Size = cameraOrthogonalSize;
        }

        OnRotationChange(this, rotation);
    }

}
