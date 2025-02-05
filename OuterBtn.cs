using Godot;
using System;
using System.Collections.Generic;
using static Godot.Image;
[Tool]
public partial class OuterBtn : TextureButton
{
    public enum outer_state
    {
        OFF,
        BLINK,
        ON
    }
    // Called when the node enters the scene tree for the first time.
    [Export]
    public int id { get; set; } = 0;

    [Signal]
    public delegate void OuterBtnPressedEventHandler(int id);

    [Signal]
    public delegate void OuterBtnReleasedEventHandler(int id);

    public override void _Ready()
    {
        this.AddToGroup("subscribed_to_store");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Pressed()
    {
        base._Pressed();
        GD.Print(id);
        EmitSignal(nameof(OuterBtnPressed), id);
    }
}
