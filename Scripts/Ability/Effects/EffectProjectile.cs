using Godot;

public partial class EffectProjectile : TaskBase {
    readonly Ability ability;
    readonly PackedScene projectileTemplate = GD.Load<PackedScene>(ResourcePaths.Projectile);
    readonly ProjectileUnit projectile;
    Vector3 targetPosition;

    public EffectProjectile(Vector3 targetPosition, NavigationUnit unit, Ability ability) {
        type = TaskType.Ability;
        this.targetPosition = targetPosition;
        this.unit = unit;
        this.ability = ability;
        projectile = projectileTemplate.Instantiate<ProjectileUnit>();
        projectile.UpdateValues(this.targetPosition, this.ability.Attributes.velocity);
        projectile.OnCollideEvent += () => {
            isForceFinished = true;
        };
        unit.AddChild(projectile);
        unit.Player.DebugLog("[AbilityProjectile.StartTask] New Projectile to " + targetPosition, true);
    }
}
