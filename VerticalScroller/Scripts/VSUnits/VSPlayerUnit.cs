
using Godot;
public partial class VSPlayerUnit : VSUnit {

    readonly PackedScene projectileScene = GD.Load<PackedScene>(ResourcePaths.Projectile);
    readonly EffectBaseDTO projectileEffect = new() {
        velocity = 1f,
    };

    Timer attackTimer = new() {
        OneShot = false,
    };

    public override void _Ready() {
        base._Ready();
        AttributesExport attributes = GetAttributes();
        Player.AxisInputHandler.InputAxisType = AxisType.XAxis;
        attackTimer.WaitTime = attributes.AttackSpeed;
        AddChild(attackTimer);
        attackTimer.Timeout += OnAttack;
        attackTimer.Start();
    }

    void OnAttack() {
        ProjectileUnit projectile = projectileScene.Instantiate<ProjectileUnit>();

        AbilityCastContext context = new(AbilityCastTypes.Position, this);
        projectile.InitializeEffectUnit(projectileEffect, context);
        Player.AddChild(projectile);
        projectile.Scale = projectile.Scale.Magnitude(1.5f);
        projectile.GlobalPosition = GlobalPosition;
        projectile.SetTargetPosition(GlobalPosition.WithZ(-1000));

        void OnCollide(Unit enemyUnit) {
            AttributesExport enemyUnitAttributes = enemyUnit.GetAttributes();
            enemyUnit.UnitAttributes.ApplyDamage(GetAttributes().BaseDamage);
            enemyUnit.OverheadLabel.Text = enemyUnitAttributes.HitPoints + " / " + enemyUnitAttributes.MaxHitPoints;
        }

        projectile.OnCollideEvent -= OnCollide;
        projectile.OnCollideEvent += OnCollide;
    }
}