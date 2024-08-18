using System;
using Godot;

public partial class VSUnit : AxisUnit {

    [Export]
    Vector2 PlayAreaLimitX = new(-1, 1f);

    readonly UnitAttributesDTO defaultUnitAttributes = new() {
        movementSpeed = 2,
        maxHitPoints = 100,
        attackSpeed = 1,
        armor = 0,
        baseDamage = 10,
        attackCastDuration = 0,
        attackRange = 25,
    };

    public override void _Ready() {
        base._Ready();
        UnitAttributes.InitializeValues(defaultUnitAttributes);
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

    public void RemoveUnit() {
        // TODO Implement a way to remove the unit safely
        QueueFree();
    }
}
