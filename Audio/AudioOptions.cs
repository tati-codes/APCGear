using APCGear.Audio;
using AudioSwitcher.AudioApi.Session;
using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class AudioOptions : OptionButton
{

	[Signal]
	public delegate void AudioSelectedEventHandler(int process_id, string label);
	public override void _Ready()
	{
		update();
		Bus.Subscribe<AudioSessionsRefresh, AudioSessionsRefreshArgs>((args) => CallDeferred("update"));
		this.ItemSelected += (long idx) =>
		{
			var item = GetItemId((int)idx);
			var name = GetItemText((int)idx);
			EmitSignal(SignalName.AudioSelected, item, name);
		};
	}

	public void update()
	{
		this.Clear();
		AddItem("None", 0);
		foreach (var items in AudioHandler.sessions)
		{
			AddItem(items.DisplayName, items.ProcessId);
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
