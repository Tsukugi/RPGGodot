using Godot;

public partial class EffectMultiProjectileOverHead : EffectBase {
    PackedScene projectileTemplate = GD.Load<PackedScene>(ResourcePaths.Projectile);
    int activeProjectiles = 0;
    float horizontalPositionOffset = 1f;

    public override void StartTask() {
        if (abilityCastContext.TargetPosition is not Vector3 targetPosition) return;
        unit.Player.DebugLog("[EffectMultiProjectileOverHead] New Projectile to " + targetPosition);
        activeProjectiles = attributes.numberOfInstances;
        for (int i = 0; i < attributes.numberOfInstances; i++) {
            ProjectileUnit projectile = NewEffectActor<ProjectileUnit>(
                   projectileTemplate,
                   unit.Player,
                   unit.GlobalPosition.AddToY(0.5f));
            projectile.SetTargetDirection(unit.GlobalPosition.AddToX(GetHorizontalProjectilePosition(i)).AddToY(1f));

            projectile.OnCollideEvent += (collider) => {
                activeProjectiles--;
                collider.Attributes.ApplyDamage(attributes.damageAmount);
            };

            projectile.OnTargetReachedEvent += (position) => {
                projectile.SetTargetDirection(targetPosition);
            };
        }
        base.StartTask();
    }

    public override void OnTaskProcess() {
        base.OnTaskProcess();
        if (activeProjectiles > 0) return;
        OnTaskCompleted();
    }

    float GetHorizontalProjectilePosition(int index) {
        float direction = index % 2 == 0 ? 1 : -1; // We use -1 for even and 1 for odd
        return horizontalPositionOffset * index * direction;
    }
}
