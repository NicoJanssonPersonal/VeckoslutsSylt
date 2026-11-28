using Godot;
using System;

public partial class Scythe : Tools
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
			Vector3 hitPoint = _raycast.GetCollisionPoint();

			if (collider is Weed)
			{
				_isPickingUp = true;
				_animPlayer.SpeedScale = 1f;
				_animPlayer.Play("swipe");

				_audioStreamPlayer3D.Play();
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
						nearbyWeed.TakeDamage(40);
					}
				}
			}
		}
	}
}
