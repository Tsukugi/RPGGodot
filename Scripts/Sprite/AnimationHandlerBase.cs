
using Godot;

public class AnimationHandlerBase {
    private string animationPrefix;
    public string AnimationPrefix { get => animationPrefix; set => animationPrefix = value; }
    protected AnimatedSprite3D animatedSprite;
    protected string GetName(string id) {
        string name = animationPrefix + id;
        return name;
    }
}
