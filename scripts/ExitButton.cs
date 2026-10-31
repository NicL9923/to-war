using Godot;

public partial class ExitButton : Button
{
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
	
	public void _on_pressed()
	{
		GetTree().Quit();
	}
}
