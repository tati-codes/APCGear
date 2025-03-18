using APCEvents.APCOut;
using APCGear.Audio;
using Godot;
using System;

public partial class Indicator : TextureRect
{
    // Called when the node enters the scene tree for the first time.
    public static readonly CompressedTexture2D connected = GD.Load<CompressedTexture2D>("res://assets/yes.png");
    public static readonly CompressedTexture2D not_connected = GD.Load<CompressedTexture2D>("res://assets/no.png");

	public override void _Ready()
	{
        set_text(State.Instance.connected_to_apc);
		Bus.Subscribe<AudioSessionsRefresh, AudioSessionsRefreshArgs>((args) => set_text(Pad.Instance.handler.connected));
	}

	public void set_text(bool connection)
	{
        if (connection)
        {
            if (!State.Instance.connected_to_apc) {
                Pad.Instance.handler.try_connect();
            }
            this.Texture = connected;
        }
        else
        {
            //Bus.Publish<IsConnectedEvent, IsConnectedEventArgs>(new IsConnectedEventArgs() { connected = Pad.Instance.handler.connected});
            this.Texture = not_connected;
        }
    }


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
