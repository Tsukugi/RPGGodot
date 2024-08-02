using Godot;


public partial class EffectProjectile : EffectBase {
    PackedScene projectileTemplate = GD.Load<PackedScene>(ResourcePaths.Projectile);
    ProjectileUnit projectile;

    public override void StartTask() {
        projectile = NewEffectActor<ProjectileUnit>(projectileTemplate, unit.Player, unit.GlobalPosition.AddToY(0.5f));
        projectile.OnCollideEvent += (collider) => {
            OnTaskCompleted();
        };
        base.StartTask();
    }

}
