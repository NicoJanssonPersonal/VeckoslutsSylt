using Godot;
using System.Linq;

public partial class Player : CharacterBody3D
{
	private Camera3D _cam;
	private Node3D _head;
	private CollisionShape3D _colShape;
	private CapsuleShape3D _capsule;

	public const float Speed = 5.0f;
	public const float CrouchSpeed = 2.5f;
	public const float JumpVelocity = 4.5f;

	private const float StandHeight = 2.0f;
	private const float CrouchHeight = 1.0f;
	private const float CrouchLerpSpeed = 10.0f;

	private float _standHeadY;
	private float _crouchHeadY;
	private bool _isCrouching = false;
    [Export] private RayCast3D _rayCast {get; set;}
    private Tools[] _tools;
    private Tools _currentTool;
    private int _currentToolIndex;
	
    public override void _Ready()
	{
		Settings.LoadData();
		_head = GetNode<Node3D>("HEAD");
		_cam = GetNode<Camera3D>("HEAD/Camera3D");
		_colShape = GetNode<CollisionShape3D>("CollisionShape3D");
		_capsule = (CapsuleShape3D)_colShape.Shape;

		_standHeadY = _head.Position.Y;
		_crouchHeadY = _standHeadY - (StandHeight - CrouchHeight);

		_cam.Current = true;
		Input.MouseMode = Input.MouseModeEnum.Captured; 

        _tools = _rayCast.GetChildren().OfType<Tools>().ToArray();
        _currentToolIndex = 0;
        SetTool(_currentToolIndex);
	}

	public override void _PhysicsProcess(double delta)
	{
		float dt = (float)delta;

		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * dt;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor() && !_isCrouching)
		{
			velocity.Y = JumpVelocity;
		}
		if (Input.IsActionJustPressed("action"))
		{
			_currentTool.useTool();
		}	
		if (Input.IsActionJustPressed("crouch"))
		{
			if (_isCrouching)
			{
				_isCrouching = false;
			}
			else
			{
				_isCrouching = true;
			}
		}

		float targetHeight = _isCrouching ? CrouchHeight : StandHeight;
		float targetHeadY = _isCrouching ? _crouchHeadY : _standHeadY;

		_capsule.Height = Mathf.Lerp(_capsule.Height, targetHeight, CrouchLerpSpeed * dt);

		Vector3 colPos = _colShape.Position;
		colPos.Y = _capsule.Height / 2.0f;
		_colShape.Position = colPos;

		Vector3 headPos = _head.Position;
		headPos.Y = Mathf.Lerp(headPos.Y, targetHeadY, CrouchLerpSpeed * dt);
		_head.Position = headPos;

		float currentSpeed = _isCrouching ? CrouchSpeed : Speed;
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
		Vector3 direction = (_head.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		if (IsOnFloor())
		{
			if (direction != Vector3.Zero)
			{
				velocity.X = direction.X * currentSpeed;
				velocity.Z = direction.Z * currentSpeed;
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, currentSpeed);
				velocity.Z = Mathf.MoveToward(Velocity.Z, 0, currentSpeed);
			}
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (GetTree().Paused || Input.MouseMode != Input.MouseModeEnum.Captured)
			return;

		if (@event is InputEventMouseMotion mouseMotion)
		{
			float sens = Settings.MouseSensitivity;

			_head.RotateY(-mouseMotion.Relative.X * sens);
			_cam.RotateX(-mouseMotion.Relative.Y * sens);

			Vector3 camRot = _cam.Rotation;
			camRot.X = Mathf.Clamp(camRot.X, Mathf.DegToRad(-80.0f), Mathf.DegToRad(75.0f));
			_cam.Rotation = camRot;
		}

        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            if (mouseEvent.ButtonIndex == MouseButton.WheelUp)
            {
                _currentToolIndex++;
                if (_currentToolIndex > _tools.Count())
                    _currentToolIndex = 0;
                
                SetTool(_currentToolIndex);
            }
            else if (mouseEvent.ButtonIndex == MouseButton.WheelDown)
            {
                _currentToolIndex--;
                if (_currentToolIndex < 0)
                    _currentToolIndex = _tools.Count() - 1;

                SetTool(_currentToolIndex);
            }
        }
	}

    public void SetTool(int index)
    {
        if (index < 0 || index >= _tools.Length)
            return;

        for (int i = 0; i < _tools.Length; i++)
            _tools[i].Visible = (i == index);

        _currentTool = _tools[index];
    }
}
