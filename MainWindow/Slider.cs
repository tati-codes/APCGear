using APC;
using APCEvents.APCOut;
using Godot;
using System;

public partial class Slider : VSlider
{
	[Export]
	public sliders id { get; set; } = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Bus.Subscribe<SliderChangedEvent, SliderChangedEventArgs>(resolve_event);
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

	public void changeValue(double newValue)
	{
		this.Value = newValue;
	}

	public void resolve_event(SliderChangedEventArgs action)
	{
		if (action.id == (int)id)
		{
			CallDeferred("changeValue", Convert.ToDouble(action.value));		
		}
	}
}