using APCEvents.Color;
using APCEvents.UI;
using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

public partial class CustomLoopVisibility : Control
{
	public override void _Ready()
    //Only Show If Previous Filled And Not Looped
    {
        Bus.Subscribe<ColorTransitionChangedEvent, ColorTransitionChangedEventArgs>((args) => /* show next id */ CallDeferred("gather", args.index, args.color));
        Bus.Subscribe<BtnSelectedEvent, BtnSelectedEventArgs>(args =>
        {
            if (State.Instance.selected_btn.colorTransitions.color_type == TransitionType.CUSTOM_LOOP ||
                State.Instance.selected_btn.colorTransitions.color_type == TransitionType.CYCLE_ALL ||
                State.Instance.selected_btn.colorTransitions.color_type == TransitionType.CYCLE_SOLIDS 
             )
            {
                CallDeferred("react");
            }
        });
    }
    public void gather(int index, int color)
	{
        if (index == 6) return;
        var nodes = GetChildren();
        List<int> result = new();
        for (int i = 0; i < nodes.Count; i++)
        {
            if (i == index)
            {
                result.Add(-2);
                continue;
            };
            ColorOptions opt = nodes[i] as ColorOptions;
            if (opt != null)
            {
                result.Add(opt.button.Selected);
            }
        }
        if (!result.Contains(color)) { 
                ((ColorOptions)nodes[index + 1]).Show();
        } 
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public void react()
    {
        var selected = State.Instance.selected_btn;
        var seq = selected.colorTransitions.sequence;
        var nodes = GetChildren();
        for (int i = 0; i < nodes.Count; i++)
        {
            if (i >= seq.Length)
            {
                ((ColorOptions)nodes[i]).Hide();
            } else
            {
                ((ColorOptions)nodes[i]).button.Select(seq[i]);
                ((ColorOptions)nodes[i]).Show();
            }
        }
    }
    public override void _Process(double delta)
	{
	}
}
