
using System;
using Godot;

public class ActorAnimationHandler : AnimationHandlerBase {
    public ActorAnimationHandler(AnimatedSprite3D sprite) {
        animatedSprite = sprite;
    }

    public UnitRenderDirection GetRenderDirectionFromVector(Vector2 direction) {
        UnitRenderDirection unitRenderDirection = UnitRenderDirection.Down;

        double directionDegrees = VectorUtils.GetRotationFromDirection(direction);

        if (directionDegrees.IsBetween(0, 45)) unitRenderDirection = UnitRenderDirection.Right;
        if (directionDegrees.IsBetween(45, 90)) unitRenderDirection = UnitRenderDirection.UpRight;
        if (directionDegrees.IsBetween(90, 135)) unitRenderDirection = UnitRenderDirection.Up;
        if (directionDegrees.IsBetween(135, 180)) unitRenderDirection = UnitRenderDirection.UpLeft;
        if (directionDegrees.IsBetween(180, 225)) unitRenderDirection = UnitRenderDirection.Left;
        if (directionDegrees.IsBetween(225, 270)) unitRenderDirection = UnitRenderDirection.DownLeft;
        if (directionDegrees.IsBetween(270, 315)) unitRenderDirection = UnitRenderDirection.Down;
        if (directionDegrees.IsBetween(315, 360)) unitRenderDirection = UnitRenderDirection.DownRight;
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
