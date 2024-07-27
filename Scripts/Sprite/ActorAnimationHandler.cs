
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

        return unitRenderDirection;
    }

    public void ApplyAnimation(UnitRenderDirection inputFaceDirection) {
        switch (inputFaceDirection) {
            case UnitRenderDirection.Down: PlayAnimation("Down"); break;
            case UnitRenderDirection.Up: PlayAnimation("Up"); break;
            case UnitRenderDirection.Left: PlayAnimation("Left"); break;
            case UnitRenderDirection.Right: PlayAnimation("Right"); break;
            case UnitRenderDirection.UpLeft: PlayAnimation("UpLeft"); break;
            case UnitRenderDirection.UpRight: PlayAnimation("UpRight"); break;
            case UnitRenderDirection.DownLeft: PlayAnimation("DownLeft"); break;
            case UnitRenderDirection.DownRight: PlayAnimation("DownRight"); break;
        }
    }

    void PlayAnimation(string name) {
        try {
            animatedSprite.Play(GetName(name));
        } catch (Exception e) {
            GD.PrintErr(e);
        }
    }

}
