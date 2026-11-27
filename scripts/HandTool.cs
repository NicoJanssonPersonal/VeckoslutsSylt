using Godot;
using System;

public partial class HandTool : Node3D
{
	[Export] private AudioStreamPlayer3D _audioStreamPlayer3D;
	[Export] private RayCast3D _raycast;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_audioStreamPlayer3D.VolumeDb = -40.0f;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void pickUpWeed()
	{
		if (_raycast.IsColliding())
		{
			GodotObject collider = _raycast.GetCollider();
			Vector3 hitPoint = _raycast.GetCollisionPoint();
			Vector3 hitNormal = _raycast.GetCollisionNormal();

			/*if (collider is Weed weed)
			{
				weed.TakeDamage(10);
			}*/

			GD.Print($"Looking at: {collider} at {hitPoint}");
			_audioStreamPlayer3D.Play();
		}
	}
}
