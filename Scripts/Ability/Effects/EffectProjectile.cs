using Godot;


public partial class EffectProjectile : EffectBase {
    readonly PackedScene projectileTemplate = GD.Load<PackedScene>(ResourcePaths.Projectile);
    ProjectileUnit projectile;

    public override void StartTask() {
        if (abilityCastContext.TargetPosition is not Vector3 targetPosition) return;
        projectile = NewEffectActor<ProjectileUnit>(projectileTemplate, unit.Player, unit.GlobalPosition.AddToY(0.5f));
        projectile.SetTargetPosition(targetPosition);

        void onCollideEvent(Unit collider) => OnTaskCompleted();
        projectile.OnCollideEvent -= onCollideEvent;
        projectile.OnCollideEvent += onCollideEvent;

        base.StartTask();
    }

}
