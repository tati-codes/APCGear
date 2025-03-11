using APCGear.Audio;
using AudioSwitcher.AudioApi.Session;
using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class AudioOptions : OptionButton
{
//TODO selected button should have an audio option in the audio section to allow it to be made a mute toggle for the audio

	[Signal]
	public delegate void AudioSelectedEventHandler(int process_id, string label);
	public Process selectedReferece = Process.nullProcess;
	public override void _Ready()
	{
		update();
		Bus.Subscribe<AudioSessionsRefresh, AudioSessionsRefreshArgs>((args) => CallDeferred("update"));
		this.ItemSelected += (long idx) =>
		{
			var item = GetItemId((int)idx);
			var name = GetItemText((int)idx);
            selectedReferece = new Process() { name = name, id = item };
			EmitSignal(SignalName.AudioSelected, item, name);
		};
	}

	public void update()
	{
		this.Clear();
		AddItem("None", 0);
		AddItem("Master", 1);
		var processes = AudioHandler.processes;
		foreach (var item in processes)
		{
			AddItem(item.name, item.id);
			if (item == selectedReferece) {
				Select(ItemCount - 1);
			}
		}
		if (!processes.Contains(selectedReferece) && selectedReferece != Process.nullProcess)
		{
			AddItem(selectedReferece.name, selectedReferece.id);
			Select(ItemCount - 1);
		}
		if (selectedReferece.id == 1) Select(1);
	}
}
