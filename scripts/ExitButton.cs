using Godot;

public partial class ExitButton : Button
{
	public void _on_pressed()
	{
		GetNode<MainMenu>("../..").RequestQuit();
	}
}
