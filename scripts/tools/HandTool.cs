using Godot;
using System;

public partial class HandTool : Tools
{

	public override void _Ready()
	{
		base._Ready();
	}
	public override void useTool()
	{
		if (_isPickingUp)
			return;

		if (_raycast.IsColliding())
		{
			GodotObject collider = _raycast.GetCollider();

			if (collider is Weed weed)
			{
				// 2. Lock picking up before starting the animation
				_isPickingUp = true;

				//GD.Print($"Looking at: {collider} at {hitPoint}");
				_audioStreamPlayer3D.Play();

				_animPlayer.SpeedScale = 1f; // 1 = 1.75 seconds
				_animPlayer.Play("take");
				weed.TakeDamage(10);
			}


		}
	}
}
