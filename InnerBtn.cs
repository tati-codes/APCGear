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
        [inner_state.RED] = (normal: GD.Load<CompressedTexture2D>("res://assets/red.png"), pressed: GD.Load<CompressedTexture2D>("res://assets/red_pressed.png")),
        [inner_state.YELLOW] = (normal: GD.Load<CompressedTexture2D>("res://assets/yellow.png"), pressed: GD.Load<CompressedTexture2D>("res://assets/yellow_pressed.png")),
    };

    [Export]
    public inner_state color { get {
            return this._color;
        } set {
            this._color = value;
            this.change_color(this._color);
        }  
    }   

    [Signal]
    public delegate void BtnPressedEventHandler(int id);
    
    [Signal]
    public delegate void BtnReleasedEventHandler(int id);

    public override void _Ready()
	{
        Bus.Subscribe<BtnPressedEvent, BtnPressedEventArgs>((BtnPressedEventArgs args) => check_id(args.Id,_Pressed));
        Bus.Subscribe<BtnReleasedEvent, BtnReleasedEventArgs>((BtnReleasedEventArgs args) => check_id(args.Id, _Released));  
    }

    private void change_color(inner_state new_color)
    {
        if (new_color == inner_state.GREEN || new_color == inner_state.RED || new_color == inner_state.YELLOW || new_color == inner_state.OFF) { 
            this.TextureNormal = buttonTexts[new_color].normal;
            this.TexturePressed = buttonTexts[new_color].pressed;
        }
    } 
    public override void _Pressed()
    {
        GD.Print(id, " pressed");
        base._Pressed();
    }
    public void _Released()
    {
        GD.Print(id, " released");
        Bus.Publish<BtnSelectedEvent, BtnSelectedEventArgs>(new BtnSelectedEventArgs() { id = id });
    }
    public void check_id(int args_id, System.Action callback)
    {
        if (args_id == id)
        {
            callback();
        }
    }
}
