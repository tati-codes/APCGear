using APCGear;
using APCGear.KeyAction;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class KeysCont : VBoxContainer
{
	// Called when the node enters the scene tree for the first time.
	public Keys charkey;
	public List<Keys> mods
    {
        get
        {
			var mods = new List<Keys>();	
            var nodes = GetTree().GetNodesInGroup("mods");
            foreach (Mods node in nodes)
            {
                mods.Add(node.key);
            }
			return mods;
        }
    }
	public Hotkey hotkey { get {
			return new Hotkey(charkey.value, mods.Select(key => key.value).ToArray()); 
		} }
	public override void _Ready()
	{
		//Bus.Subscribe<SelectedCharChanged, SelectedCharChangedArgs>(emitShortcutChange);
		//Bus.Subscribe<SelectedModsChanged, SelectedModsChangedArgs>(emitShortcutChange);
	}
	//private void emitShortcutChange(SelectedModsChangedArgs wha) => emitShortcutChange();
	//private void emitShortcutChange(SelectedCharChangedArgs wha) => emitShortcutChange();

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_button_pressed() => Bus.Publish<ShortcutChanged, ShortcutEventArgs>(new ShortcutEventArgs() { Hotkey = hotkey });
}
