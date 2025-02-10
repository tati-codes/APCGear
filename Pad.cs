using Godot;
using APC;
using APCGear.APCOut;
using static APC.APCMiniListener;
using System;
using Melanchall;
using APCGear.APCIn;

public partial class Pad : Node
{
    public APC.Handler handler;
    public static Pad Instance;
    public override void _Ready()
    {
        Instance = this;
        handler = new APC.Handler();
        handler.clear();
        handler.connection += (bool payload) => Bus.Publish<IsConnectedEvent, IsConnectedEventArgs>(new IsConnectedEventArgs() { connected = payload });
        handler.btn_pressed += (int id) => Bus.Publish<BtnPressedEvent, BtnPressedEventArgs>(new BtnPressedEventArgs() { Id = id });
        handler.btn_released += (int id) => Bus.Publish<BtnReleasedEvent, BtnReleasedEventArgs>(new BtnReleasedEventArgs() { Id = id });
        handler.slider_changed += (int id, int value) => Bus.Publish<SliderChangedEvent, SliderChangedEventArgs>(new SliderChangedEventArgs() { id = id, value = value });
    }

}

