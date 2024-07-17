using Godot;

public partial class AbilityProjectile : TaskBase {
    Vector3 targetPosition;
    Vector2 unitPositionOnTaskCheck;
    PackedScene projectileTemplate = GD.Load<PackedScene>(ResourcePaths.Projectile);

    CharacterBody3D projectile;

    public AbilityProjectile(Vector3 targetPosition, NavigationUnit unit) {
        type = TaskType.Ability;
        this.targetPosition = targetPosition;
        this.unit = unit;
        projectile = projectileTemplate.Instantiate<CharacterBody3D>();
        unit.AddChild(projectile);
    }

    public override void StartTask() {
        base.StartTask();
        unit.Player.DebugLog("[Ability.StartTask] Casting to " + targetPosition);
    }
    public override bool CheckIfCompleted() {
        return base.CheckIfCompleted();
    }

    public override void OnTaskProcess() {
        KinematicCollision3D collision3D = projectile.MoveAndCollide(projectile.GlobalPosition.DirectionTo(targetPosition));
    }

    public override void OnTaskCompleted() {
        base.OnTaskCompleted();
    }

}
