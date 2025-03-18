using APC;
using APCGear.Audio;
using Godot;
using System;

public partial class SliderRoot : Control
{
	[Export]
    public sliders id { get; set; } = 0;
	[Export]
	public Slider slider { get; set; }
	[Export]
	public AudioOptions processSelector { get; set; }
    public override void _Ready()
	{
        this.processSelector.AudioSelected += (int id, string name) => {
            Bus.Publish<LinkSliderToProcess, LinkSliderArgs>(new LinkSliderArgs() { process = new Process() { id = id, name = name }, slider = (int)id });
        };
		var process = State.Instance.slider_table[id].process;
    }
}
