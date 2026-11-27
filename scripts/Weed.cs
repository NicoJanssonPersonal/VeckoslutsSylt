using Godot;
using System;

public partial class Weed : StaticBody3D
{
	private int maxLife = 15;
	private int _currentLife;
	
	public override void _Ready()
	{
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
			EventBus.Instance.EmitSignal(EventBus.SignalName.WeedPicked);
            EventBus.Instance.EmitSignal(EventBus.SignalName.MoneyEarned, 0.10f);
			QueueFree();
		}
	}
}
