
using Godot;

public class ActorAnimationHandler : AnimationHandlerBase {
    public ActorAnimationHandler(AnimatedSprite3D sprite) {
        animatedSprite = sprite;
    }

    public void ApplyAnimation(InputFaceDirection inputFaceDirection) {
        switch (inputFaceDirection) {
            case InputFaceDirection.Down: animatedSprite.Play(GetName("Down")); break;
            case InputFaceDirection.Up: animatedSprite.Play(GetName("Up")); break;
            case InputFaceDirection.Left: animatedSprite.Play(GetName("Left")); break;
            case InputFaceDirection.Right: animatedSprite.Play(GetName("Right")); break;
        }
    }

}
