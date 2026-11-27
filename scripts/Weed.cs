using Godot;
using System;

public partial class Weed : StaticBody3D
{
	private Gamemanager gameManager;
	private int maxLife = 15;
	private int _currentLife;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//gameManager = GetNode<Gamemanager>("/GAMEMANAGER");
		_currentLife = maxLife;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	public void TakeDamage(int damage)
	{
		_currentLife -= damage;
		if (_currentLife <= 0)
		{
			//gameManager.UpdateWeedsPicked();
			QueueFree();
		}
	}
}
