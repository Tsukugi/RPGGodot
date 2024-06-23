using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class UnitTask : Node {
    NavigationUnit unit;
    TaskBase currentTask = null;
    Stack<TaskBase> tasks = new();
    Timer taskCheckTimer = new() {
        OneShot = false,
        WaitTime = 1f + new System.Random().NextDouble(),
    };


    public override void _Ready() {
        base._Ready();
        unit = this.FindNavigationUnit();

        // Timer
        AddChild(taskCheckTimer);
        taskCheckTimer.Timeout += OnTaskCheck;
        taskCheckTimer.Start();
    }

    void OnTaskCheck() {
        if (currentTask.CheckIfCompleted()) CompleteTask();
    }

    void CompleteTask() {
        currentTask = tasks.Pop();
    }

    public void Add(TaskBase newTask) {
        tasks.Append(newTask);
    }

    void Attack(Unit target) {
        GD.Print("[UnitTask.Add] Attacking " + target.Name);
    }
    void AttackMove(Vector3 position) {
        GD.Print("[UnitTask.Add] Moving and Attacking if necessary");
        unit.NavigateTo(position);
    }
    void Interact(Unit target) {
        GD.Print("[UnitTask.Add] Interacting with " + target.Name);
    }


}

