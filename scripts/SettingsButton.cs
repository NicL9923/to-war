using Godot;

public partial class SettingsButton : Button
{
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}
	
	public void _on_pressed()
	{
		GD.Print("Settings clicked");
	}
}
