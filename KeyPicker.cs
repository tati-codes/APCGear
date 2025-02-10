using APCGear;
using APCGear.KeyAction;
using Godot;
using state_types;
using System;

public partial class KeyPicker : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	PackedScene charScene { get; set; }
	[Export]
	PackedScene modsScene { get; set; }
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void clear_children()
	{
        foreach (var item in this.GetChild(1).GetChildren())
        {
            item.QueueFree();
        }
    }

    public void populate(Hotkey hotkey)
	{
        var cont = this.GetChild(1);
		var charsNode = charScene.Instantiate<CharMenu>();
		charsNode.Select((int)hotkey.charKey);
		cont.AddChild(charsNode);
        foreach (var item in hotkey.mods)
        {
			var tempMods = modsScene.Instantiate<Mods>();
			cont.AddChild(tempMods);
			tempMods.Select((int)item);
        }
	}
}
