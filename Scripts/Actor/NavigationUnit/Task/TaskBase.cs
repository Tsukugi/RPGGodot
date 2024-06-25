public enum TaskType {
    Move, AttackMove, Attack, Interact,
}

public abstract class TaskBase {
    protected NavigationUnit unit;
    protected TaskType type;
    protected float navigationTargetSafeDistanceRadius = 3f;
    protected bool isAlreadyStarted = false;
    private bool isForceFinished = false;
    public TaskType Type { get => type; }
    public bool IsAlreadyStarted { get => isAlreadyStarted; }
    protected bool IsForceFinished { get => isForceFinished; }

    /// <summary>
    /// This method does the initial actions when the task is started
    /// </summary>
    public virtual void StartTask() {
        isAlreadyStarted = true;
    }
    /// <summary>
    /// Use this method to update some actions on every Task's Timer interval
    /// </summary>
    public abstract void OnTaskProcess();
    /// <summary>
    /// This method is to be checked every check interval, and checks if the task can be completed
    /// </summary>
    public virtual bool CheckIfCompleted() {
        return isForceFinished;
    }
    /// <summary>
    /// This method is to be executed when the completion check has passed, use it to clean the state of the unit.
    /// </summary>
    public virtual void OnTaskCompleted() {
        isForceFinished = true;
    }
}
