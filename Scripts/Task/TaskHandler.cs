using System.Collections.Generic;
using Godot;

public partial class TaskHandler : Node {
    protected TaskBase currentTask = null;
    protected Queue<TaskBase> tasks = new();

    protected Timer taskProcessTimer = new() {
        OneShot = false,
        WaitTime = 0.5f,
    };

    public int Count { get => tasks.Count; }
    public TaskBase CurrentTask { get => currentTask; }
    public delegate void OnTaskEvent();
    public event OnTaskEvent OnAllTasksCompleted;

    public override void _Ready() {
        base._Ready();
        AddChild(taskProcessTimer);
        taskProcessTimer.Timeout += OnTaskProcess;
    }

    protected virtual void OnTaskProcess() {
        if (tasks.Count <= 0) {
            OnAllTasksCompleted?.Invoke();
            return;
        }

        currentTask ??= tasks.Peek();

        if (currentTask.CheckIfCompleted()) {
            CompleteTask();
        } else if (currentTask.IsAlreadyStarted) {
            currentTask.OnTaskProcess();
        } else {
            currentTask.StartTask();
        }
    }


    void CompleteTask() {
        currentTask?.OnTaskCompleted();
        currentTask = null;
        if (tasks.Count > 0) tasks.Dequeue();
    }

    public void AddTask(TaskBase newTask) {
        tasks.Enqueue(newTask);
    }

    public void PriorityAddTask(TaskBase newTask) {
        Queue<TaskBase> newQueue = new();
        newQueue.Enqueue(newTask);
        foreach (var item in tasks) {
            newQueue.Enqueue(item);
        }
        tasks = newQueue;
        currentTask = newTask;
    }

    public void PriorityReplaceTask(TaskBase newTask) {
        currentTask = newTask;
    }

    public void ClearAll() {
        currentTask = null;
        tasks.Clear();
        CompleteTask();
    }


    public virtual void StartTimer() {
        taskProcessTimer.Start();
    }

    public virtual void StopTimer() {
        taskProcessTimer.Stop();
    }

}

