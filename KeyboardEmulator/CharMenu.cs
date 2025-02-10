using APCGear;
using APCGear.KeyAction;
using Godot;
using System;

public partial class CharMenu : OptionButton
{
    // Called when the node enters the scene tree for the first time.
    public Keys key { get { return Keys.charKeys[(int)this.Selected]; } }

    public override void _Ready()
    {
        foreach (var item in Keys.charKeys)
        {
            this.AddItem(item.label, (int)item.value);
        }
        this.ItemSelected += (long index) => GetParent<KeysCont>().charkey = key;
    }
	// Called every frame. 'delta' is the elapsed time since the previous frame.
}
