using Godot;
using System;

public partial class Enemy : PathFollow2D
{
	public bool IsBlocked { get { return GetParent() != path; } }
	float speed = 5.0f;
	int attack = 10;
	double attackSpeed = 1.0;
	double attackProgress = 0.0;
	[Export]
	private Path2D path;
	[Export]
	private Health health;

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

		attackProgress += delta;

		if (attackProgress > attackSpeed) {
			GetParent().GetParent<Unit>().Damage(attack);
			attackProgress = 0;
		}
	}

	public void Damage(int damage) {
		health.Modify(-damage);
		if (health.CurrentHealth == 0) {
			Die();
		}
	}

	public void UnBlock() {
		Reparent(path);
	}

	void Die() {
		QueueFree();
	}
}
