
using Godot;

public class EffectAnimationHandler : AnimationHandlerBase {
    public EffectAnimationHandler(AnimatedSprite3D sprite) {
        animatedSprite = sprite;
    }

    public void ApplyAnimation(UnitActionState state) {
        switch (state) {
            case UnitActionState.Attack: animatedSprite.Play("Attack"); break;
        }
    }

}
