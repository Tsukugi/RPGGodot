using System.Collections.Generic;
using Godot;

public partial class UnitTask : Node {
    NavigationUnit unit;
    TaskBase currentTask = null;
    Queue<TaskBase> tasks = new();

    Timer taskProcessTimer = new() {
        OneShot = false,
        WaitTime = 0.5f,
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
        //! Debug
        string text = unit.Name + " \n "
            + unit.Attributes.HitPoints + "/" + unit.Attributes.MaxHitPoints + " \n "
            + unit.AlertArea.AlertState + " \n ";
        if (currentTask != null) {
            text += currentTask.Type.ToString();
            if (unit.UnitCombat.IsInCombat) text += " " + unit.UnitCombat.TargetName;
        }
        unit.OverheadLabel.Text = text + " \n " + tasks.Count.ToString();
        //! EndDebug

        if (tasks.Count <= 0) return;

        if (currentTask == null) currentTask = tasks.Peek();

        if (currentTask.CheckIfCompleted()) {
            unit.Player.DebugLog("[OnTaskCheck] " + currentTask.Type + " has been completed");
            CompleteTask();
        } else if (currentTask.IsAlreadyStarted) {
            unit.Player.DebugLog("[OnTaskCheck] " + unit.Name + "'s " + currentTask.Type + " Task is processed");
            currentTask.OnTaskProcess();
        } else {
            unit.Player.DebugLog("[OnTaskCheck] " + currentTask.Type + " Task has started");
            currentTask.StartTask();
        }
    }


    void CompleteTask() {
        if (currentTask != null) currentTask.OnTaskCompleted();
        currentTask = null;
        if (tasks.Count > 0) tasks.Dequeue();
    }

    public void Add(TaskBase newTask) {
        tasks.Enqueue(newTask);
        unit.Player.DebugLog("[UnitTask.Add] " + newTask.Type + " has been added for " + unit.Name + ". Current tasks length: " + tasks.Count);
    }

    public void PriorityRunTask(TaskBase newTask) {
        Queue<TaskBase> newQueue = new();
        newQueue.Enqueue(newTask);
        foreach (var item in tasks) {
            newQueue.Enqueue(item);
        }
        tasks = newQueue;
        currentTask = newTask;
        unit.Player.DebugLog("[UnitTask.PriorityAdd] " + newTask.Type + " has been added for " + unit.Name + " as highest priority. Current tasks length: " + tasks.Count);
    }

    public void ClearAll() {
        currentTask = null;
        tasks.Clear();
        CompleteTask();
    }

}

