using Godot;
using System;

public partial class Player : CharacterBody3D
{	
	private float sens = 0.001f;
	private Camera3D _cam;
	private Node3D _head;

	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	public override void _Ready()
	{
		_head = GetNode<Node3D>("HEAD");
		_cam = GetNode<Camera3D>("HEAD/Camera3D");

		_cam.Current = true;
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}
	public override void _PhysicsProcess(double delta)
	{
		if (!IsMultiplayerAuthority()) return;

		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
		Vector3 direction = (_head.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
	public override void _UnhandledInput(InputEvent @event)
	{
		if (!IsMultiplayerAuthority()) return;

		if (@event is InputEventMouseMotion mouseMotion)
		{
			_head.RotateY(-mouseMotion.Relative.X * sens);
			_cam.RotateX(-mouseMotion.Relative.Y * sens);

			Vector3 camRot = _cam.Rotation;
			camRot.X = Mathf.Clamp(camRot.X, Mathf.DegToRad(-40.0f), Mathf.DegToRad(60.0f));
			_cam.Rotation = camRot;
		}
	}
}
