using Godot;
using System;

public partial class GameManager : Node3D
{
	[Export]
	private Node3D selectedPlayerActor = null;

	public Node3D SelectedPlayerActor { get => selectedPlayerActor; set => selectedPlayerActor = value; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
