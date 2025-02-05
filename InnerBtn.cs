using Godot;
using System;
using System.Collections.Generic;
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
        this.AddToGroup("subscribed_to_store");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
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
        base._Pressed();
        GD.Print(id);
        EmitSignal(nameof(BtnPressed), id);
    }
}
