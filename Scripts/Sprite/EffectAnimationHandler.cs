
using Godot;

public class EffectAnimationHandler : AnimationHandlerBase {
    public EffectAnimationHandler(AnimatedSprite3D sprite) {
        animatedSprite = sprite;
    }

    public void ApplyAnimation(ActionState state) {
        switch (state) {
            case ActionState.Attack: animatedSprite.Play("Attack"); break;
        }
    }

}
