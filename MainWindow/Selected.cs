using APCEvents.UI;
using Godot;
using System;

public partial class Selected : TextureRect
{
	// Called when the node enters the scene tree for the first time.
	public Godot.Vector2 offset = new(-16,-8);
	public override void _Ready()
	{
        Bus.Subscribe<BtnPublishPositionEvent, Position>((args) =>
		{
			this.Show();
			GD.Print("Selected ", args.pos);
			this.GlobalPosition = args.pos + offset;
		});
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
    }
}
