using Godot;
using System;

public partial class Slider : VSlider
{
	[Export]
	public int id { get; set; } = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.AddToGroup("subscribed_to_store");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void _on_check_box_toggled(bool muted)
	{
		this.Editable = !muted;
		//TODO send mute 
	}
}