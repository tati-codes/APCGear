using APCGear;
using APCGear.KeyAction;
using Godot;
using System;

public partial class Mods : OptionButton
{
    // Called when the node enters the scene tree for the first time.

    public Keys key { get { return Keys.modKeys[(int)this.Selected]; } }
    public override void _Ready()
	{
        foreach (var item in Keys.modKeys)
        {
            this.AddItem(item.label, (int)item.value);
        }
        this.AddToGroup("mods");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
