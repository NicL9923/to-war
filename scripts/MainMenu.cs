using Godot;

public partial class MainMenu : Control
{
	private bool _quitting;

	public override void _Ready()
	{
		GetTree().AutoAcceptQuit = false;
	}

	public override void _ExitTree()
	{
		GetTree().AutoAcceptQuit = true;
	}

	public override void _Notification(int what)
	{
		if (what == NotificationWMCloseRequest)
			RequestQuit();
	}

	public async void RequestQuit()
	{
		if (_quitting)
			return;
		_quitting = true;
		GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stop();
		// Godot 4.7 releases stopped audio on a later audio mix, not synchronously.
		await ToSignal(GetTree().CreateTimer(0.25), SceneTreeTimer.SignalName.Timeout);
		GetTree().Quit();
	}
}
