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
	private int maxBlock;
	[Export]
	public Node2D Blocking { private set; get;}

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
	}

	void Block(Enemy enemy) {
		enemy.Reparent(Blocking);
	}
}
