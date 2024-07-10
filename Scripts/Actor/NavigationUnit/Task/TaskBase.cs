public enum TaskType {
    Move, Attack, Hide, Cast //,AttackMove, Interact,
}

public abstract class TaskBase {
    protected NavigationUnit unit;
    protected TaskType type;
    protected bool isAlreadyStarted = false;
    protected bool isForceFinished = false;
    public TaskType Type { get => type; }
    public bool IsAlreadyStarted { get => isAlreadyStarted; }
    public bool IsForceFinished { get => isForceFinished; }

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
        if (isForceFinished) return;
        OnTaskCompletedEvent?.Invoke(this);
        isForceFinished = true;
    }

    public delegate void OnTaskEvent(TaskBase task);
    public event OnTaskEvent OnTaskCompletedEvent;
}
