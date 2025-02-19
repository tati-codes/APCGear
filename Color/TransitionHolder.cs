using APC;
using APCEvents.Color;
using Godot;
using System;

public partial class TransitionHolder : HBoxContainer
{
    [Export]
    public int TransitionId { get; set; }

    [Export]
    public ColorOptions idle { get; set; }
    [Export]
    public ColorOptions pressed { get; set; }
    [Export]
    public ColorOptions released { get; set; }

    public ComplexColorOptionChosenArgs gathered { get
        {
            return new ComplexColorOptionChosenArgs() { 
                key = (inner_state)TransitionId,
                pressed = pressed.button.Selected > -1 ? (inner_state)pressed.button.Selected : null,
                released = released.button.Selected > -1 ? (inner_state)released.button.Selected : null,
            };
        } }

    public override void _Ready()
    {
        this.AddToGroup("TransitionHolders");
        idle.button.Select(TransitionId);
        idle.button.Disabled = true;
        pressed.button.Disabled = true;
        Bus.Subscribe<ToggleMiddleRow, Toggle>((args) => {
            pressed.button.Disabled = !args.show;
        });
        released.button.ItemSelected += gather;
        pressed.button.ItemSelected += gather;
    }

    public void gather(long color) => Bus.Publish<ComplexColorOptionChosenEvent, ComplexColorOptionChosenArgs>(gathered);


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
