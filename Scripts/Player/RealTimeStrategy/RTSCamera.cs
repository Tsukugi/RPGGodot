using Godot;

public partial class RTSCamera : Node {
    RealTimeStrategyPlayer player;
    [Export]
    bool enableDrag = true;
    [Export]
    bool enableMoveOnEdge = true;
    [Export]
    bool enableZoom = true;
    // Camera
    Vector2 cameraDragStartPosition = Vector2.Zero;
    Vector2 cameraDragCurrentPosition = Vector2.Zero;
    Vector2 cameraEdgeMovingDirection = Vector2.Zero;


    public override void _Ready() {
        base._Ready();
        player = this.TryFindParentNodeOfType<RealTimeStrategyPlayer>();
    }

    public override void _Input(InputEvent @event) {
        base._Input(@event);
        if (!player.IsFirstPlayer()) return;

        if (@event is InputEventMouseMotion) {
            if (enableDrag) OnDragMotion();
            if (enableMoveOnEdge) OnMoveOnEdgeMotion();
        }

        if (@event is InputEventMouseButton eventMouseButton) {
            if (enableZoom) OnZoomStart(eventMouseButton);
            if (enableDrag) OnDragStart(eventMouseButton);
        }
    }


    void OnMoveOnEdgeMotion() {
        cameraEdgeMovingDirection = player.Camera.GetEdgeMovingDirection();
    }

    void OnDragMotion() {
        if (Input.IsMouseButtonPressed(MouseButton.Middle)) {
            cameraDragCurrentPosition = player.Camera.GetViewport().GetMousePosition();
        }
    }

    void OnDragStart(InputEventMouseButton eventMouseButton) {
        if (eventMouseButton.ButtonIndex == MouseButton.Middle) {
            if (eventMouseButton.Pressed) {
                player.DebugLog("[RealTimeStrategyPlayer._Input]: Start Camera Drag");
                cameraDragStartPosition = player.Camera.GetViewport().GetMousePosition();
            } else {
                player.DebugLog("[RealTimeStrategyPlayer._Input]: End Camera Drag");
                // Reset
                cameraDragStartPosition = Vector2.Zero;
                cameraDragCurrentPosition = Vector2.Zero;
            }
        }
    }

    void OnZoomStart(InputEventMouseButton eventMouseButton) {
        if (eventMouseButton.ButtonIndex == MouseButton.WheelDown) {
            player.Camera.Zoom(CameraZoomDirection.ZoomOut, 1);
        } else if (eventMouseButton.ButtonIndex == MouseButton.WheelUp) {
            player.Camera.Zoom(CameraZoomDirection.ZoomIn, 1);
        }
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (!player.IsFirstPlayer()) return;

        // Camera WASD move
        Vector2 axis = NavigationInputHandler.GetAxis();
        if (axis != Vector2.Zero) {
            player.Camera.AxisMove(axis.Rotate(CameraBase.CameraRotationAxisOffset), (float)delta);
        }

        // Camera Middle mouse button drag move
        if (cameraDragCurrentPosition != Vector2.Zero) {
            player.Camera.AxisMove((cameraDragCurrentPosition - cameraDragStartPosition).Rotate(CameraBase.CameraRotationAxisOffset).Normalized(), (float)delta);
        }

        // Camera edge move
        if (cameraEdgeMovingDirection != Vector2.Zero) {
            player.Camera.AxisMove(cameraEdgeMovingDirection.Normalized(), (float)delta);
        }
    }

}