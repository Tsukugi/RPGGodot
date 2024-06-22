using Godot;


public partial class AttackCollisionArea : Area3D {

    CollisionShape3D meleeCollisionArea;

    public CollisionShape3D MeleeCollisionArea { get => meleeCollisionArea; }

    public override void _Ready() {
        meleeCollisionArea = GetNode<CollisionShape3D>(AxisNodeNames.MeleeCollisionArea);
    }

}
