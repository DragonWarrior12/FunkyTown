using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

public partial class Unit : Node2D
{
	[Export]
	private Area2D blockArea;
	[Export]
	private Area2D attackArea;
	[Export]
	private int maxBlock;
	[Export]
	public Node2D Blocking { private set; get;}
	[Export]
	private Health health;
	int attack = 10;
	double attackSpeed = 1.0;
	double attackProgress = 0.0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		#region block
		if (Blocking.GetChildCount() < maxBlock) {
			List<Enemy> enemies = new List<Enemy>();

			foreach (Area2D area in blockArea.GetOverlappingAreas()) {
				enemies.Add(area.GetParent<Enemy>());
			}

			enemies.Sort(Enemy.SortByProgress);

			foreach (Enemy enemy in enemies.Where((Enemy e) => { return !e.IsBlocked; })
										.Take(maxBlock - Blocking.GetChildCount())) {
				Block(enemy);
			}
		}
		#endregion

		#region attack
			Enemy target = null;

			if (Blocking.GetChildCount() > 0) {
				target = Blocking.GetChild<Enemy>(0);
			} else {
				List<Enemy> enemies = new List<Enemy>();

				foreach (Area2D area in attackArea.GetOverlappingAreas()) {
					enemies.Add(area.GetParent<Enemy>());
				}
				
				if (enemies.Count > 0) {
					enemies.Sort(Enemy.SortByProgress);
					target = enemies[0];
				}
			}

			if (target != null) {
				attackProgress += delta;
				if (attackProgress > attackSpeed) {
					target.Damage(attack);
					attackProgress = 0;
				}
			} else {
				attackProgress = 0;
			}
		#endregion
	}

	public void Damage(int damage) {
		health.Modify(-damage);
		if (health.CurrentHealth == 0) {
			Die();
		}
	}

	void Block(Enemy enemy) {
		enemy.Reparent(Blocking);
	}

	void Die() {
		foreach (Enemy enemy in Blocking.GetChildren()) {
			enemy.UnBlock();
		}
		QueueFree();
	}
}
