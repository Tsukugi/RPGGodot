public enum TaskType {
    Move, AttackMove, Attack, Interact,
}

public abstract class TaskBase {
    protected NavigationUnit unit;
    protected TaskType type;
    protected bool isAlreadyStarted = false;
    public TaskType Type { get => type; }
    public bool IsAlreadyStarted { get => isAlreadyStarted; }

    /// <summary>
    /// This method tells the initial actions when the task is started
    /// </summary>
    public abstract void StartTask();
    /// <summary>
    /// This method is to be checked every check interval, and tells if the task can be completed
    /// </summary>
    public abstract bool CheckIfCompleted();
}
