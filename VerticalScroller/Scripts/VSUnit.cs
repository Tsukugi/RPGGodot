using System;
using Godot;

public partial class VSUnit : AxisUnit {
    readonly PackedScene projectileScene = GD.Load<PackedScene>(ResourcePaths.Projectile);
    [Export]
    Vector4 PlayAreaLimit = new(-10, -1.5f, 10, 1.5f);

    Timer attackTimer = new() {
        OneShot = false,
    };

    readonly EffectBaseDTO projectileEffect = new() {
        velocity = 1f,
    };
    readonly UnitAttributesDTO unitAttributesDTO = new() {
        movementSpeed = 1,
        maxHitPoints = 100,
        attackSpeed = 1,
        armor = 1,
        baseDamage = 20,
        attackCastDuration = 0,
        attackRange = 25,
    };

    public override void _Ready() {
        base._Ready();
        if (!Player.IsFirstPlayer()) return;
        Attributes.UpdateValues(unitAttributesDTO);
        attackTimer.WaitTime = Attributes.AttackSpeed;
        AddChild(attackTimer);
        attackTimer.Timeout += OnAttack;
        attackTimer.Start();
    }

    public override void _PhysicsProcess(double delta) {

        // * We will force position if we go too far from the Play Area
        if (!GlobalPosition.X.IsBetween(PlayAreaLimit.Y, PlayAreaLimit.W)) {
            GlobalPosition = GlobalPosition.WithX(GetInsidePlayAreaPosition(GlobalPosition.X, PlayAreaLimit.Y, PlayAreaLimit.W));
        } else if (!GlobalPosition.Z.IsBetween(PlayAreaLimit.X, PlayAreaLimit.Z)) {
            GlobalPosition = GlobalPosition.WithZ(GetInsidePlayAreaPosition(GlobalPosition.Z, PlayAreaLimit.X, PlayAreaLimit.Z));
        }

        base._PhysicsProcess(delta);
    }

    public void UpdateUnit(VSEnemies unitInfo) {
        Attributes.UpdateValues(unitInfo.attributes);
        UnitRender.UpdateSprite(VSPaths.SpriteFrames + unitInfo.textureName + ".tres");
    }

    static float GetInsidePlayAreaPosition(float value, float a, float b) {
        float limitedValue = value.LimitToRange(a, b);
        return (Math.Abs(limitedValue) - 0.01f) * value.Normalize();
    }

    void OnAttack() {
        ProjectileUnit projectile = projectileScene.Instantiate<ProjectileUnit>();

        AbilityCastContext context = new(AbilityCastTypes.Position, this);
        projectile.InitializeEffectUnit(projectileEffect, context);
        projectile.Scale *= 2;
        Player.AddChild(projectile);
        projectile.GlobalPosition = GlobalPosition;
        projectile.SetTargetPosition(GlobalPosition.WithZ(-1000));

        void OnCollide(Unit enemyUnit) {
            enemyUnit.Attributes.ApplyDamage(Attributes.BaseDamage);
            enemyUnit.OverheadLabel.Text = enemyUnit.Attributes.HitPoints + " / " + enemyUnit.Attributes.MaxHitPoints;
        }
        projectile.OnCollideEvent -= OnCollide;
        projectile.OnCollideEvent += OnCollide;
    }
}