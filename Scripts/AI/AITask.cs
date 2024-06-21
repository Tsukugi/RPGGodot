
using Godot;

public partial class AITask : Node {
    AITaskName taskName = AITaskName.Idle;
    


}

enum AITaskName {
    Idle,
    Move,
    Interact,
}