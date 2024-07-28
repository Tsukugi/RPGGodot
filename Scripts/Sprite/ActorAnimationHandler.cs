
using System;
using Godot;

public class ActorAnimationHandler : AnimationHandlerBase {
    readonly int numberOfSpriteFaces = 8;
    readonly float centerFaceOffset = 45 / 2;
    public ActorAnimationHandler(AnimatedSprite3D sprite) {
        animatedSprite = sprite;
    }

    public UnitRenderDirection GetRenderDirectionFromVector(Vector2 direction) {
        UnitRenderDirection unitRenderDirection = UnitRenderDirection.Down;

        double sideSize = 360 / numberOfSpriteFaces; // A full round divided onto the number of faces (8 faces)
        double directionDegrees = Math.Abs(VectorUtils.GetRotationFromDirection(direction));

        for (int i = 0; i < numberOfSpriteFaces; i++) {
            if (directionDegrees - centerFaceOffset >= (i * sideSize) + centerFaceOffset) {
                unitRenderDirection = (UnitRenderDirection)i;
            }
        }
        GD.Print(directionDegrees + " - " + sideSize + " - " + unitRenderDirection);
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
