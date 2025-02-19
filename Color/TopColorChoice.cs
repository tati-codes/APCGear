using APCEvents.Color;
using APCEvents.UI;
using Godot;
using state_types;
using System;
using System.Collections.Generic;

public partial class TopColorChoice : OptionButton
{
	[Export]
	public TabContainer tabContainer { get; set; }

	public readonly Dictionary<TransitionType, string> groups = new()
	{
		[TransitionType.COMPLEX] = "ComplexColorOption",
		[TransitionType.CUSTOM_LOOP] = "ColorControlVboxColor"
    };
     public override void _Ready()
	{
		this.ItemSelected += resolve;
		tabContainer.CurrentTab = 0;
		Bus.Subscribe<BtnSelectedEvent, BtnSelectedEventArgs>((args) => CallDeferred("on_selected"));
		Bus.Subscribe<ColorTransitionChangedEvent, ColorTransitionChangedEventArgs>((args => CallDeferred("gather")));
    }
    public void resolve(long id)
	{
		var selected = State.Instance.selected_btn;
		if (selected != null)
		{
			TransitionType enumed = (TransitionType)id;
			switch (enumed)
			{
				case TransitionType.CYCLE_ALL:
					Bus.Publish<SetColorEvent, ColorTransitions>(selected.type == btn_type.INNER ? ColorTransitions.inner_all : ColorTransitions.outer_all);
					break;
				case TransitionType.CYCLE_SOLIDS:
                    Bus.Publish<SetColorEvent, ColorTransitions>(selected.type == btn_type.INNER ? ColorTransitions.inner_solids : ColorTransitions.outer_solids);
                    break;
				case TransitionType.CUSTOM_LOOP:
					//FIXME Check if there's already a custom transition, if so fill the enums
					Bus.Publish<SetColorEvent, ColorTransitions>(new ColorTransitions() { color_type = TransitionType.CUSTOM_LOOP, sequence = new int[1] {0}, type = selected.type});;
					break;
				default:
					break;
			}
		}
		tabContainer.CurrentTab = (int)id;
    }
    public void gather()
	{
        var nodes = GetTree().GetNodesInGroup(groups[TransitionType.CUSTOM_LOOP]);
		List<int> ints = new List<int>();	
		foreach (var node in nodes)
		{
            OptionButton button = node as OptionButton;
			if (button != null && button.Selected > -1)
			{
				ints.Add(button.Selected);
			}
		}
		Bus.Publish<SetColorEvent, ColorTransitions>(new ColorTransitions() {color_type = TransitionType.CUSTOM_LOOP, sequence = ints.ToArray(), type = State.Instance.selected_btn.type});	
    }
	public void on_selected()
	{
        if (State.Instance.selected_btn != null && State.Instance.selected_btn.colorTransitions != null)
        {
            var btn = State.Instance.selected_btn;
            var ttrpe = btn.colorTransitions.color_type;
            this.Select((int)ttrpe);
        }
    }
    public override void _Process(double delta)
	{
	}
}
