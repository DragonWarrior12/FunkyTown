using Godot;
using System;

public partial class Enemy : PathFollow2D
{
	public bool IsBlocked { get { return GetParent() != path; } }
	float speed = 5.0f;
	[Export]
	private Path2D path;

	public static int SortByProgress(Enemy a, Enemy b) {
		if (a.ProgressRatio > b.ProgressRatio) return 1;
		if (a.ProgressRatio < b.ProgressRatio) return -1;
		return 0;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (!IsBlocked) { Progress += speed * (float)delta; return; }
	}

	public void UnBlock() {
		Reparent(path);
	}
}
