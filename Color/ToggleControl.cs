using APCEvents.Color;
using APCEvents.UI;
using Godot;
using System;
using System.Linq;

public partial class ToggleControl : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	public OptionButton singleColor { get; set; }
	public override void _Ready()
	{
		singleColor.ItemSelected += (long idx) =>
		{
            Bus.Publish<SetColorEvent, ColorTransitions>(new ColorTransitions() { color_type = TransitionType.TOGGLE, sequence = new int[2] { 0, (int)idx } });
        };
		Bus.Subscribe<BtnSelectedEvent, BtnSelectedEventArgs>(args =>
		{
			var selected = State.Instance.selected_btn.colorTransitions;         
			if (selected.color_type == TransitionType.TOGGLE)
			{
				singleColor.Select(selected.sequence.Last());
			}
		});
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
