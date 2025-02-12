using APCGear.Color;
using APCGear.UI;
using Godot;
using state_types;
using System;
using System.Collections.Generic;

public partial class TopColorChoice : OptionButton
{
	[Export]
	public VBoxContainer customs { get; set; }

	public readonly string Group = "ColorControlVboxColor";
     public override void _Ready()
	{
		this.ItemSelected += resolve;
		Bus.Subscribe<BtnSelectedEvent, BtnSelectedEventArgs>((args) => CallDeferred("on_selected"));
		Bus.Subscribe<ColorTransitionChangedEvent, ColorTransitionChangedEventArgs>((args => CallDeferred("gather")));
    }
    public void resolve(long id)
	{
		var selected = State.Instance.selected;
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
				case TransitionType.CUSTOM:
					Bus.Publish<SetColorEvent, ColorTransitions>(new ColorTransitions() { color_type = TransitionType.CUSTOM, sequence = Array.Empty<int>(), type = selected.type});;
					break;
				default:
					break;
			}
		}
    }
	public void gather()
	{
        var nodes = GetTree().GetNodesInGroup(Group);
		List<int> ints = new List<int>();	
		foreach (var node in nodes)
		{
            OptionButton button = node as OptionButton;
			if (button != null && button.Selected > -1)
			{
				ints.Add(button.Selected);
			}
		}
		Bus.Publish<SetColorEvent, ColorTransitions>(new ColorTransitions() {color_type = TransitionType.CUSTOM, sequence = ints.ToArray(), type = State.Instance.selected.type});	
    }
	public void on_selected()
	{
        if (State.Instance.selected != null && State.Instance.selected.colorTransitions != null)
        {
            var btn = State.Instance.selected;
            var ttrpe = btn.colorTransitions.color_type;
            this.Select((int)ttrpe);
        }
    }
    public override void _Process(double delta)
	{
	}
}
