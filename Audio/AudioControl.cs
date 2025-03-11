using APC;
using APCGear.Audio;
using Godot;
using System;
//TODO auto assign sliders on ready unless they have an assigned process already that is found
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
		options.AudioSelected += (int id, string name) => Bus.Publish<LinkSliderToProcess, LinkSliderArgs>(new LinkSliderArgs() { process = new Process() { id = id, name = name }, slider = (int)slider_id});
		label.Text = Enum.GetName(typeof(sliders), slider_id);
		var state = State.Instance;
		if (state.slider_table[slider_id].process != Process.nullProcess)
		{
			options.selectedReferece = state.slider_table[slider_id].process;
			options.update();	
        }
	}
}
