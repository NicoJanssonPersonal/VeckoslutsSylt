using Godot;
using System;

public partial class Scythe : Node3D
{

	[Export] private AudioStreamPlayer3D _audioStreamPlayer3D;

	[Export] private RayCast3D _raycast;

	private AnimationPlayer _animPlayer;

	private bool _isPickingUp = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_animPlayer = GetNode<AnimationPlayer>("Lia/AnimationPlayer");
		_audioStreamPlayer3D.VolumeDb = -40.0f;
		_animPlayer.AnimationFinished += OnAnimationFinished;

	}

	public void cutWeedWithScythe()
	{
		if (_isPickingUp)
			return;

		if (_raycast.IsColliding())
		{
			GodotObject collider = _raycast.GetCollider();
			Vector3 hitPoint = _raycast.GetCollisionPoint();

			if (collider is Weed)
			{
				_isPickingUp = true;
				_audioStreamPlayer3D.Play();
				_animPlayer.SpeedScale = 1f;
				_animPlayer.Play("swipe");

				// Defines the swing radius in meters
				float swingRadius = 1.5f;

				// Creates a temporary sphere query at the hit point
				var spaceState = GetWorld3D().DirectSpaceState;
				var query = new PhysicsShapeQueryParameters3D
				{
					Shape = new SphereShape3D { Radius = swingRadius },
					Transform = new Transform3D(Basis.Identity, hitPoint),
					CollideWithBodies = true  // Set true if Weed uses StaticBody3D/CharacterBody3D
				};

				// Find all overlapping colliders (up to 32)
				var results = spaceState.IntersectShape(query, 32);

				foreach (var result in results)
				{
					if (result["collider"].AsGodotObject() is Weed nearbyWeed)
					{
						nearbyWeed.TakeDamage(20);
					}
				}
			}
		}
	}
	private void OnAnimationFinished(StringName animName)
	{
		// 3. Unlock once the "take" animation finishes
		if (animName == "swipe")
		{
			_isPickingUp = false;
		}
	}
}
