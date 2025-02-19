using APC;
using APCGear.Audio;
using Godot;
using System;

public partial class AudioControl : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public sliders slider_id { get; set; }

    [Export]
    public AudioOptions options { get; set; }

    [Export]
    public Label label { get; set; }
    public override void _Ready()
	{
		options.AudioSelected += (int id, string name) =>
		{
				Bus.Publish<LinkSliderToProcess, LinkSliderArgs>(new LinkSliderArgs() { process = id, slider = (int)slider_id});
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
