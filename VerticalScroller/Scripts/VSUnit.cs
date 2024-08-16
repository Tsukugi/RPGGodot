using System;
using Godot;

public partial class VSUnit : AxisUnit {

    readonly PackedScene projectileScene = GD.Load<PackedScene>(ResourcePaths.Projectile);
    [Export]
    Vector2 PlayAreaLimitX = new(-1.2f, 1.2f);

    Timer attackTimer = new() {
        OneShot = false,
    };

    readonly EffectBaseDTO projectileEffect = new() {
        velocity = 1f,
    };
    readonly UnitAttributesDTO unitAttributesDTO = new() {
        movementSpeed = 1,
        maxHitPoints = 100,
        attackSpeed = 0.5,
        armor = 1,
        baseDamage = 20,
        attackCastDuration = 0,
        attackRange = 25,
    };

    public override void _Ready() {
        base._Ready();
        UnitAttributes.InitializeValues(unitAttributesDTO);
        if (!Player.IsFirstPlayer()) return;
        AttributesExport attributes = GetAttributes();
        Player.AxisInputHandler.InputAxisType = AxisType.XAxis;
        attackTimer.WaitTime = attributes.AttackSpeed;
        AddChild(attackTimer);
        attackTimer.Timeout += OnAttack;
        attackTimer.Start();
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        // * We will force position if we go too far from the Play Area
        if (!GlobalPosition.X.IsBetween(PlayAreaLimitX.X, PlayAreaLimitX.Y)) {
            GlobalPosition = GlobalPosition.WithX(GetInsidePlayAreaPosition(GlobalPosition.X, PlayAreaLimitX.X, PlayAreaLimitX.Y));
        }
    }

    public void UpdateUnit(VSEnemy unitInfo) {
        UnitAttributes.InitializeValues(unitInfo.attributes);
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
        Player.AddChild(projectile);
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
