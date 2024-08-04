using Godot;


public partial class EffectProjectile : EffectBase {
    PackedScene projectileTemplate = GD.Load<PackedScene>(ResourcePaths.Projectile);
    ProjectileUnit projectile;

    public override void StartTask() {
        if (abilityCastContext.TargetPosition is not Vector3 targetPosition) return;
        projectile = NewEffectActor<ProjectileUnit>(projectileTemplate, unit.Player, unit.GlobalPosition.AddToY(0.5f));
        projectile.SetTargetDirection(targetPosition);
        projectile.OnCollideEvent += (collider) => {
            OnTaskCompleted();
        };
        base.StartTask();
    }

}
