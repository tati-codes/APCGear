using APCGear.APCOut;
using APCGear.UI;
using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using static Godot.Image;
[Tool]
public partial class InnerBtn : TextureButton
{
    public enum inner_state
    {
        OFF,
        GREEN,
        GREEN_BLINK,
        RED,
        RED_BLINK,
        YELLOW,
        YELLOW_BLINK
    }
	// Called when the node enters the scene tree for the first time.
    [Export]
    public int id { get; set; } = 0;

    private inner_state _color = inner_state.OFF;
    private static Dictionary<inner_state, (CompressedTexture2D normal, CompressedTexture2D pressed)> buttonTexts = new Dictionary<inner_state, (CompressedTexture2D normal, CompressedTexture2D pressed)>() {
        [inner_state.OFF] = (normal: GD.Load<CompressedTexture2D>("res://assets/off.png"), pressed: GD.Load<CompressedTexture2D>("res://assets/off_pressed.png")),
        [inner_state.GREEN] = (normal: GD.Load<CompressedTexture2D>("res://assets/green.png"), pressed: GD.Load<CompressedTexture2D>("res://assets/green_pressed.png")),
        [inner_state.YELLOW_BLINK] = (normal: GD.Load<CompressedTexture2D>("res://assets/yellow_blink.png"), pressed: GD.Load<CompressedTexture2D>("res://assets/yellow_pressed.png")),
        [inner_state.RED_BLINK] = (normal: GD.Load<CompressedTexture2D>("res://assets/red_blink.png"), pressed: GD.Load<CompressedTexture2D>("res://assets/red_pressed.png")),
        [inner_state.GREEN_BLINK] = (normal: GD.Load<CompressedTexture2D>("res://assets/green_blink.png"), pressed: GD.Load<CompressedTexture2D>("res://assets/green_pressed.png")),
        [inner_state.RED] = (normal: GD.Load<CompressedTexture2D>("res://assets/red.png"), pressed: GD.Load<CompressedTexture2D>("res://assets/red_pressed.png")),
        [inner_state.YELLOW] = (normal: GD.Load<CompressedTexture2D>("res://assets/yellow.png"), pressed: GD.Load<CompressedTexture2D>("res://assets/yellow_pressed.png")),
    };


    public override void _Ready()
	{
        //Bus.Subscribe<BtnPressedEvent, BtnPressedEventArgs>((BtnPressedEventArgs args) => check_id(args.Id,_Pressed));
        //Bus.Subscribe<BtnReleasedEvent, BtnReleasedEventArgs>((BtnReleasedEventArgs args) => check_id(args.Id, _Released));
        Bus.Subscribe<BtnSelectedEvent, BtnSelectedEventArgs>((args) => check_id(args.id, () => Bus.Publish<BtnPublishPositionEvent, Position>(new() { pos = this.GlobalPosition })));
        Bus.Subscribe<ChangeColorInUiEvent, BtnSelectedEventArgs>((args) => check_id(args.id, () => change_color()));
    }

    public void change_color()
    {
        var new_color = (inner_state)State.Instance.buttons[id].colorTransitions.currentColor;

        GD.Print("here");
        //TODO make the ui change color
        this.TextureNormal = buttonTexts[new_color].normal;
        this.TexturePressed = buttonTexts[new_color].pressed;
    } 
    public override void _Pressed()
    {
        Bus.Publish<BtnSelectedEvent, BtnSelectedEventArgs>(new BtnSelectedEventArgs() { id = id });
        base._Pressed();
    }
    public void _Released()
    {
        GD.Print(id, " released");
    }
    public void check_id(int args_id, System.Action callback)
    {
        if (args_id == id)
        {
            GD.Print("calling: ", args_id, " from: ", id);
            callback();
        }
    }
}
