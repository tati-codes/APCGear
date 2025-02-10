using Godot;
using System;

public partial class AddAnother : Button
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	Node keysContainer { get; set; } = null;
	[Export]
	string path { get; set; } = null;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    public override void _Pressed()
    {
		if (keysContainer != null && path != null)
		{
			var packed = GD.Load<PackedScene>(path);
			var scene = packed.Instantiate<Mods>();
			keysContainer.AddChild(scene);
		}
    }
}
