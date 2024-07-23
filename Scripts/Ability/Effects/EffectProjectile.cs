using Godot;


public partial class EffectProjectile : EffectBase {
    PackedScene projectileTemplate = GD.Load<PackedScene>(ResourcePaths.Projectile);
    ProjectileUnit projectile;

    public override void StartTask() {
        unit.Player.DebugLog("[EffectProjectile.StartTask] New Projectile to " + target.GlobalPosition, true);
        projectile = NewEffectActor<ProjectileUnit>(projectileTemplate, unit.Player, unit.GlobalPosition.WithY(0.5f));
        projectile.OnCollideEvent += (collider) => {
            OnTaskCompleted();
        };
        base.StartTask();
    }

}
