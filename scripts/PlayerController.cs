using Godot;

public partial class PlayerController : Node
{
	[Export] public Node3D PitchPivot;
	[Export] public float Sensitivity = 0.01f;
	[Export] private CharacterMovement _characterMovement;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
		_characterMovement.Move(inputDir);

		if (Input.IsActionJustPressed("move_jump"))
		{
			_characterMovement.Jump();
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			// Rotate the body (Twist) left/right
			_characterMovement.RotateY(-mouseMotion.Relative.X * Sensitivity);
			
			// Rotate the camera (Pitch) up/down
			PitchPivot.RotateX(-mouseMotion.Relative.Y * Sensitivity);

			// Clamp vertical look to prevent full rotation
			Vector3 rotation = PitchPivot.Rotation;
			rotation.X = Mathf.Clamp(rotation.X, Mathf.DegToRad(-70), Mathf.DegToRad(70));
			PitchPivot.Rotation = rotation;
		}
	}
}
