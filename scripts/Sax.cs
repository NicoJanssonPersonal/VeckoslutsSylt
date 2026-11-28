using Godot;
using System;

public partial class Sax : Node3D
{
	[Export] private AudioStreamPlayer3D _audioStreamPlayer3D;

	[Export] private RayCast3D _raycast;

	private AnimationPlayer _animPlayer;

	private bool _isPickingUp = false;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animPlayer = GetNode<AnimationPlayer>("SAXEN/AnimationPlayer");
		_audioStreamPlayer3D.VolumeDb = -40.0f;
		_animPlayer.AnimationFinished += OnAnimationFinished;

	}

	public void cutWeedWithSax()
	{
		if (_isPickingUp)
			return;

		if (_raycast.IsColliding())
		{
			GodotObject collider = _raycast.GetCollider();
			Vector3 hitPoint = _raycast.GetCollisionPoint();
			Vector3 hitNormal = _raycast.GetCollisionNormal();

			if (collider is Weed weed)
			{
				// 2. Lock picking up before starting the animation
				_isPickingUp = true;

				//GD.Print($"Looking at: {collider} at {hitPoint}");
				_audioStreamPlayer3D.Play();

				_animPlayer.SpeedScale = 1f; // 1 = 1.75 seconds
				_animPlayer.Play("klipp");
				weed.TakeDamage(20);
			}
		}
	}
	private void OnAnimationFinished(StringName animName)
	{
		// 3. Unlock once the "take" animation finishes
		if (animName == "klipp")
		{
			_isPickingUp = false;
		}
	}
}
