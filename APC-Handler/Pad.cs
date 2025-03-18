using Godot;
using APC;
using APCEvents.APCOut;
using static APC.APCMiniListener;
using System;
using Melanchall;
using APCEvents.APCIn;
using APCGear.Audio;

public partial class Pad : Node
{
    public APC.Handler handler;
    public static Pad Instance;
    public override void _Ready()
    {
        Instance = this;
        handler = new APC.Handler();
        if (handler.connected)
        {
            Bus.Publish<IsConnectedEvent, IsConnectedEventArgs>(new IsConnectedEventArgs() { connected = handler.connected });
        }
        Bus.Subscribe<AudioSessionsRefresh, AudioSessionsRefreshArgs>((args) =>
        {
            if (handler.connected != State.Instance.connected_to_apc)
            {
                if (handler.connected && !State.Instance.connected_to_apc)
                {
                    handler.try_connect();
                    return;
                }
                Bus.Publish<IsConnectedEvent, IsConnectedEventArgs>(new IsConnectedEventArgs() { connected = handler.connected });
            }
        });
        handler.connection += (bool payload) => Bus.Publish<IsConnectedEvent, IsConnectedEventArgs>(new IsConnectedEventArgs() { connected = payload });
        handler.btn_pressed += (int id) => Bus.Publish<BtnPressedEvent, BtnId>(new BtnId() { Id = id });
        handler.btn_released += (int id) => Bus.Publish<BtnReleasedEvent, BtnId>(new BtnId() { Id = id });
        handler.slider_changed += (int id, int value) => Bus.Publish<SliderChangedEvent, SliderChangedEventArgs>(new SliderChangedEventArgs() { id = id, value = value });
    }

}

