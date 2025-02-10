using APCGear.APCOut;
using APCGear.UI;
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
        Bus.Subscribe<BtnPressedEvent, BtnPressedEventArgs>((BtnPressedEventArgs args) => check_id(args.Id, _Pressed));
        Bus.Subscribe<BtnReleasedEvent, BtnReleasedEventArgs>((BtnReleasedEventArgs args) => check_id(args.Id, _Released));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Pressed()
    {
        base._Pressed();
        GD.Print(id);
    }
    public void _Released()
    {
        GD.Print(id, " released");
        Bus.Publish<BtnSelectedEvent, BtnSelectedEventArgs>(new BtnSelectedEventArgs() { id = id});
    }
    public void check_id(int args_id, System.Action callback)
    {
        if (args_id == id)
        {
            callback();
        }
    }
}
