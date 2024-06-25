
using System;
using Godot;

public class ActorAnimationHandler : AnimationHandlerBase {
    public ActorAnimationHandler(AnimatedSprite3D sprite) {
        animatedSprite = sprite;
    }

    public static UnitRenderDirection GetRenderDirectionFromVector(Vector2 direction) {
        UnitRenderDirection unitRenderDirection;

        if (Math.Abs(direction.X) > Math.Abs(direction.Y)) {
            if (direction.X > 0) unitRenderDirection = UnitRenderDirection.Right;
            else unitRenderDirection = UnitRenderDirection.Left;
        } else {
            if (direction.Y > 0) unitRenderDirection = UnitRenderDirection.Down;
            else unitRenderDirection = UnitRenderDirection.Up;
        }

        GD.Print(unitRenderDirection);
        return unitRenderDirection;
    }

    public void ApplyAnimation(UnitRenderDirection inputFaceDirection) {
        switch (inputFaceDirection) {
            case UnitRenderDirection.Down: animatedSprite.Play(GetName("Down")); break;
            case UnitRenderDirection.Up: animatedSprite.Play(GetName("Up")); break;
            case UnitRenderDirection.Left: animatedSprite.Play(GetName("Left")); break;
            case UnitRenderDirection.Right: animatedSprite.Play(GetName("Right")); break;
        }
    }

}
