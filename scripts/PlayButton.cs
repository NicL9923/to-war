using Godot;

public partial class PlayButton : Button
{
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	public void _on_pressed()
	{

		var scene = GD.Load<PackedScene>("res://main.tscn");
		GetTree().ChangeSceneToPacked(scene);
	}
}
