using Godot;

public partial class EffectMultiProjectileOverHead : EffectBase {
    PackedScene projectileTemplate = GD.Load<PackedScene>(ResourcePaths.Projectile);
    ProjectileUnit projectile;
    int activeProjectiles = 0;
    float horizontalPositionOffset = 1f;

    public override void StartTask() {
        unit.Player.DebugLog("[EffectMultiProjectile.StartTask] New Projectile to " + target.GlobalPosition);
        activeProjectiles = attributes.numberOfInstances;
        for (int i = 0; i < attributes.numberOfInstances; i++) {
            projectile = NewEffectActor<ProjectileUnit>(
                projectileTemplate,
                unit.Player,
                unit.GlobalPosition.AddToX(getHorizontalProjectilePosition(i)).AddToY(1f));

            projectile.Scale.Magnitude(2);

            projectile.OnCollideEvent += (collider) => {
                activeProjectiles--;
                collider.Attributes.ApplyDamage(attributes.damageAmount);
            };
        }
        base.StartTask();
    }

    public override void OnTaskProcess() {
        base.OnTaskProcess();
        if (activeProjectiles > 0) return;
        OnTaskCompleted();
    }

    float getHorizontalProjectilePosition(int index) {
        float direction = index % 2 == 0 ? 1 : -1; // We use -1 for even and 1 for odd
        return horizontalPositionOffset * index * direction;
    }
}
