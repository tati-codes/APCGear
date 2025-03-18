using APCEvents.Color;
using APCEvents.UI;
using Godot;
using state_types;
using System;
using System.Collections.Generic;

public partial class TopColorChoice : OptionButton
{
	//TODO fill in all the options for the choices here
	//TODO make all of those options reaction to button selected
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
		Bus.Subscribe<BtnSelectedEvent, BtnSelectedEventArgs>((args) => CallDeferred("on_btn_selected"));
		Bus.Subscribe<ColorTransitionChangedEvent, ColorTransitionChangedEventArgs>((args => CallDeferred("gather")));
    }
    public void resolve(long id) // i think this one is for event bubbling up
	{
		var selected = State.Instance.selected_btn;
		if (selected != null)
		{
			TransitionType enumed = (TransitionType)id;
			switch (enumed) //FIXME ADD A TRANSITIONTYPE THAT'S JUST SINGLE
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
				case TransitionType.TOGGLE:
					Bus.Publish<SetColorEvent, ColorTransitions>(new ColorTransitions() { color_type = TransitionType.TOGGLE, sequence = new int[2] { 0, 1 }, type = selected.type });
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
	public void on_btn_selected() 
	{
        var btn = State.Instance.selected_btn;
        var ttrpe = btn.colorTransitions.color_type;
		switch (ttrpe)
		{
			case TransitionType.TOGGLE:
				{
					switch_to(0);
                    break;
				}
			case TransitionType.CUSTOM_LOOP:
				{
                    switch_to(1);
                    break;

                }
            case TransitionType.COMPLEX:
				{
                    switch_to(2);
                    break;
				}
			default: {
					switch_to(1);
                    break;
                };
		}
    }

	public void switch_to(int idx)
	{
		this.Select(idx);
		tabContainer.CurrentTab = idx;
	}
    public override void _Process(double delta)
	{
	}
}
