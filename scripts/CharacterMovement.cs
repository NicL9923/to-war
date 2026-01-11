using Godot;

// TODO: Sprint mechanic

public partial class CharacterMovement : CharacterBody3D
{
	[Export] public float MaxSpeed = 7.0f;
	[Export] public float Acceleration = 12.0f;
	[Export] public float Friction = 15.0f;
	[Export] public float JumpVelocity = 4.5f;
	[Export] public float TerminalVelocity = 50.0f;

	private Vector2 _currentInput = Vector2.Zero;
	private bool _wantsToJump = false;

	public override void _Ready()
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Handle falling
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;

			if (velocity.Y < -TerminalVelocity)
			{
				velocity.Y = -TerminalVelocity;
			}
		}

		// Handle jumping
		if (_wantsToJump && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}
		_wantsToJump = false;

		// Handle horizontal movement
		float decayRate = _currentInput.Length() > 0 ? Acceleration : Friction;
		float accelerationForTimeChunk = 1.0f - Mathf.Exp(-decayRate * (float)delta);
		Vector3 direction = (Transform.Basis * new Vector3(_currentInput.X, 0, _currentInput.Y)).Normalized();
		Vector3 targetVelocity = direction * MaxSpeed;

		velocity.X = Mathf.Lerp(velocity.X, targetVelocity.X, accelerationForTimeChunk);
		velocity.Z = Mathf.Lerp(velocity.Z, targetVelocity.Z, accelerationForTimeChunk);

		Velocity = velocity;
		MoveAndSlide();
	}

	public void Move(Vector2 inputDir)
	{
		_currentInput = inputDir;
	}

	public void Jump()
	{
		_wantsToJump = true;
	}
}
