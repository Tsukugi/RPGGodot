using Godot;

public partial class EffectProjectile : EffectBase {
    PackedScene projectileTemplate = GD.Load<PackedScene>(ResourcePaths.Projectile);
    new ProjectileDTO attributes;

    ProjectileUnit projectile;

    public override void StartTask() {
        type = TaskType.Effect;
        unit.Player.DebugLog("[EffectProjectile.StartTask]" + unit.Name, true);
        projectile = projectileTemplate.Instantiate<ProjectileUnit>();
        projectile.OnCollideEvent += OnTaskCompleted;
        unit.Player.AddChild(projectile);
        projectile.GlobalPosition = unit.GlobalPosition.WithY(0.5f);
        projectile.UpdateValues(target.GlobalPosition, attributes.velocity);
        unit.Player.DebugLog("[EffectProjectile.StartTask] New Projectile to " + target.GlobalPosition, true);
        base.StartTask();
    }
}
