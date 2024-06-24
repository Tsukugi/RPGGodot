using System.Collections.Generic;
using Godot;

public partial class UnitTask : Node {
    NavigationUnit unit;
    TaskBase currentTask = null;
    readonly Queue<TaskBase> tasks = new();

    Timer taskProcessTimer = new() {
        OneShot = false,
        WaitTime = 1f,
    };

    public int Count { get => tasks.Count; }
    public TaskBase CurrentTask { get => currentTask; }

    public override void _Ready() {
        base._Ready();
        unit = this.TryFindParentNodeOfType<NavigationUnit>();

        AddChild(taskProcessTimer);
        taskProcessTimer.Timeout += OnTaskProcess;
        taskProcessTimer.Start();
    }

    void OnTaskProcess() {
        if (tasks.Count <= 0) return;

        if (currentTask == null) currentTask = tasks.Peek();

        if (currentTask.CheckIfCompleted()) {
            unit.Player.DebugLog("[OnTaskCheck] " + currentTask.Type + " has been completed");
            CompleteTask();
        } else if (currentTask.IsAlreadyStarted) {
            unit.Player.DebugLog("[OnTaskCheck] " + unit.Name + "'s " + currentTask.Type + " Task is processed");
            currentTask.OnTaskProcess();
        } else {
            currentTask.StartTask();
            unit.Player.DebugLog("[OnTaskCheck] " + currentTask.Type + " Task has started");
        }
    }


    void CompleteTask() {
        currentTask.OnTaskCompleted();
        currentTask = null;
        if (tasks.Count > 0) tasks.Dequeue();

    }

    public void Add(TaskBase newTask) {
        tasks.Enqueue(newTask);
        unit.Player.DebugLog("[UnitTask.Add] " + newTask.Type + " has been added for " + unit.Name + ". Current tasks length: " + tasks.Count);
    }

    public void ClearAll() {
        currentTask = null;
        tasks.Clear();
        CompleteTask();
    }
}

