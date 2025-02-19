using APCEvents.Color;
using Godot;
using System;
using System.Collections.Generic;

public partial class CustomLoopVisibility : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    //Only Show If Previous Filled And Not Looped
    {
        Bus.Subscribe<ColorTransitionChangedEvent, ColorTransitionChangedEventArgs>((args) => /* show next id */ CallDeferred("gather", args.index, args.color));
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
    public override void _Process(double delta)
	{
	}
}
