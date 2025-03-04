using APC;
using APCGear.Audio;
using Godot;
using System;
//TODO auto assign sliders on ready unless they have an assigned process already that is found
//TODO changed processId from number to text
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
		label.Text = Enum.GetName(typeof(sliders), slider_id);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
