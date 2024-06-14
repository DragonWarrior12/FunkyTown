using Godot;
using System;

public partial class Health : ProgressBar
{
	[Export]
	public double MaxHealth { get { return MaxValue; } private set { MaxValue = value; } }
	[Export]
	public double CurrentHealth { get { return Value; } private set { Value = value; } }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Modify(int value) {
		CurrentHealth += value;
	}
}
