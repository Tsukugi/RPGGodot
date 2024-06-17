
using Godot;

// ? Deprecated
public partial class SpriteHandler : Node2D
{
    public void Animate(Vector2 direction)
    {

        var animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        if (direction.Length() > 0)
        {
            animatedSprite.Play();
        }
        else
        {
            animatedSprite.Stop();
        }


        if (direction.X != 0)
        {
            animatedSprite.Animation = "right";
            // See the note below about boolean assignment.
            animatedSprite.FlipH = direction.X < 0;
            animatedSprite.FlipV = false;
        }
        else if (direction.Y != 0)
        {
            animatedSprite.Animation = "up";
            animatedSprite.FlipV = direction.Y > 0;
        }
    }

}
