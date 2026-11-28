using Godot;
using System;

public partial class Weed : StaticBody3D
{
	[Export] public int MaxLife { get; set; } = 15;
    [Export] public float Reward { get; set; } = 0.10f;
	private int _currentLife;
	
	public override void _Ready()
	{
		_currentLife = MaxLife;
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
            EventBus.Instance.EmitSignal(EventBus.SignalName.MoneyEarned, Reward);
			QueueFree();
		}
	}
}
