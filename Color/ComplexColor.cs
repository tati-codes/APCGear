using APC;
using APCEvents.Color;
using Godot;
using System;
using System.Collections.Generic;

public partial class ComplexColor : GridContainer
{
    public const string GROUP = "ComplexColorOption";
    [Export]
    public CheckBox showPressed { get; set; }
    public override void _Ready()
    {
        showPressed.Toggled += (show) => Bus.Publish<ToggleMiddleRow, Toggle>(new() { show = show });
        showPressed.Toggled += (show) => Bus.Publish<SetAdvanceOnPress, SetAdvanceOnPressArgs>(new SetAdvanceOnPressArgs() { advance = show});
        Bus.Subscribe<ComplexColorOptionChosenEvent, ComplexColorOptionChosenArgs>((args => CallDeferred("gather")));
    }

    private void showMiddleOptions(bool toggledOn)
    {
    }

    public void gather()
    {
        var nodes = GetTree().GetNodesInGroup("TransitionHolders");
        Dictionary<inner_state, (inner_state? pressed, inner_state? released)> colors = new();
        foreach (var node in nodes)
        {
            TransitionHolder singleColor = node as TransitionHolder;
            if (singleColor != null)
            {
                var comOption = singleColor.gathered;
                colors[comOption.key] = (comOption.pressed, comOption.released);
            }
        }
        ComplexColorTransition result = new ComplexColorTransition() { colors = colors };
        Bus.Publish<SetComplexColorEvent, ComplexColorTransition>( result);
    }
    public override void _Process(double delta)
	{
	}
}
