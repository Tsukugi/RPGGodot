
using Godot;

public partial class AnimationHandler
{
    private string animationPrefix;
    private AnimatedSprite3D animatedSprite;


    private string GetName(string id)
    {
        string name = animationPrefix + id;
        GD.Print(name);
        return name;
    }

    public void UpdateAnimationPrefix(string newPrefix)
    {
        animationPrefix = newPrefix;
    }

    public AnimationHandler(AnimatedSprite3D _animatedSprite, string _animationPrefix)
    {
        animationPrefix = _animationPrefix;
        animatedSprite = _animatedSprite;
    }

    public void ApplyAnimation(InputFaceDirection inputFaceDirection)
    {
        switch (inputFaceDirection)
        {
            case InputFaceDirection.Down: animatedSprite.Play(GetName("Down")); break;
            case InputFaceDirection.Up: animatedSprite.Play(GetName("Up")); break;
            case InputFaceDirection.Left: animatedSprite.Play(GetName("Left")); break;
            case InputFaceDirection.Right: animatedSprite.Play(GetName("Right")); break;
        }
    }

}
