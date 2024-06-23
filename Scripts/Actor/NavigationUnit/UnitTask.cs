using System.Collections.Generic;
using Godot;

public partial class UnitTask : Node {
    NavigationUnit unit;
    TaskBase currentTask = null;
    readonly Queue<TaskBase> tasks = new();
    Timer taskCheckTimer = new() {
        OneShot = false,
        WaitTime = 0.5f + new System.Random().NextDouble(),
    };

    public int Count { get => tasks.Count; }
    public TaskBase CurrentTask { get => currentTask; }

    public override void _Ready() {
        base._Ready();
        unit = this.TryFindNavigationUnit();

        // Timer
        AddChild(taskCheckTimer);
        taskCheckTimer.Timeout += OnTaskCheck;
        taskCheckTimer.Start();
    }

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);
        if (currentTask is null) return;
        if (currentTask.CheckIfCompleted()) return;
        currentTask.OnPhysicsProcess();
    }

    void OnTaskCheck() {
        if (tasks.Count > 0) {
            if (currentTask == null) currentTask = tasks.Peek();
            if (!(currentTask.IsAlreadyStarted || currentTask.CheckIfCompleted())) {
                currentTask.StartTask();
                unit.Player.DebugLog("[OnTaskCheck] " + currentTask.Type + " Task has started");
            }
        } else {
            unit.Player.DebugLog("[OnTaskCheck] " + unit.Name + " has finished all tasks");
        }

        if (currentTask is null) return;
        if (currentTask.CheckIfCompleted()) {
            unit.Player.DebugLog("[OnTaskCheck] " + currentTask.Type + " has been completed");
            CompleteTask();
            return;
        } else {
            currentTask.OnTaskInterval();
        }

    }


    void CompleteTask() {
        currentTask = null;
        if (tasks.Count > 0) tasks.Dequeue();
    }

    public void Add(TaskBase newTask) {
        tasks.Enqueue(newTask);
        unit.Player.DebugLog("[UnitTask.Add] " + newTask.Type + " has been added for " + unit.Name + ". Current tasks length: " + tasks.Count);
    }

    public void ClearAll() {
        tasks.Clear();
        CompleteTask();
    }
}

