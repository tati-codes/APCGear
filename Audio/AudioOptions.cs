using APCGear.Audio;
using AudioSwitcher.AudioApi.Session;
using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class AudioOptions : OptionButton
{
//TODO selected button should have an audio option in the audio section to allow it to be made a mute toggle for the audio

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
		AddItem("Master", 1);
		foreach (var items in AudioHandler.sessions)
		{
			AddItem(items.DisplayName, items.ProcessId);
		}
	}
}
