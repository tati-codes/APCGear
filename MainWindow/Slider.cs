using APC;
using APCEvents.APCOut;
using APCGear.Audio;
using Godot;
using System;

public partial class Slider : VSlider
{
	[Export]
	public SliderRoot root;

	[Export]
	public CheckBox CheckBox { get; set; }
	public APCGear.Audio.Process process { get => State.Instance.slider_table[root.id].process; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        this.DragEnded += Slider_DragEnded;
        Bus.Subscribe<SliderChangedEvent, SliderChangedEventArgs>(resolve_event);
		Bus.Subscribe<LinkSliderToProcess, LinkSliderArgs>(args =>
		{
			if ((sliders)args.slider == root.id)
			{
				force_update();
			}
		});
        force_update();

    }
    private void Slider_DragEnded(bool valueChanged)
    {
        Bus.Publish<SliderChangedEvent, SliderChangedEventArgs>(new SliderChangedEventArgs() { id = (int)root.id, value = (int)this.Value });
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public void _on_check_box_toggled(bool muted)
	{
		this.Editable = !muted;
        if (process != APCGear.Audio.Process.nullProcess)
        {
            var p = AudioHandler.getProcess(process);
			State.Instance.mute(process);
        }
	}

	public void changeValue(double newValue)
	{
		this.Value = newValue;
	}

	public void force_update()
	{
		GD.Print(process.name);
        if (process != APCGear.Audio.Process.nullProcess)
        {
            var p = AudioHandler.getProcess(process);
			GD.Print("Updating,", p);
            if (process.name == "Master")
            {
                this.Value = AudioHandler.ctrl.Volume;
            }
            else if (p != null)
            {
                this.Value = p.Volume;
            }
        }
    }

	public void resolve_event(SliderChangedEventArgs action)
	{
		if (action.id == (int)root.id)
		{
			CallDeferred("changeValue", Convert.ToDouble(action.value));		
		}
	}
}