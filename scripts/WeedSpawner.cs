using Godot;
using System;

public partial class WeedSpawner : Node
{
	[Export] public PackedScene Grass { get; set; }
	[Export] public PackedScene Bush { get; set; }

	[Export] public Vector2 SpawnAreaMin { get; set; } = new(-10, -10);
	[Export] public Vector2 SpawnAreaMax { get; set; } = new(10, 10);

		
	private Timer _spawnTimer;
	private readonly RandomNumberGenerator _rng = new();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_spawnTimer = GetNode<Timer>("SpawnTimer");
		_spawnTimer.Timeout += SpawnObject;
		_spawnTimer.Start();
	}

	private void SpawnObject()
	{
		var instance = _rng.Randf() < 0.05 ? Bush.Instantiate<Node3D>() : Grass.Instantiate<Node3D>();

		float x = _rng.RandfRange(SpawnAreaMin.X, SpawnAreaMax.X);
		float z = _rng.RandfRange(SpawnAreaMin.Y, SpawnAreaMax.Y);

		AddChild(instance);
		instance.Position = new Vector3(x, 0, z);
		instance.Rotation = new Vector3(0, _rng.Randf() * Mathf.Tau, 0);
		float scale = _rng.RandfRange(0.5f, 2.0f);
		instance.Scale = new Vector3(scale, scale, scale);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
