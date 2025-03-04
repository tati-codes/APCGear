using APCEvents;
using APCEvents.KeyAction;
using Godot;
using System;

public partial class CharMenu : OptionButton
{
    public Keys key { get { return Keys.charKeys[(int)this.Selected]; } }
    public override void _Ready()
    {
        foreach (var item in Keys.charKeys)
        {
            this.AddItem(item.label, (int)item.value);
        }
        this.ItemSelected += (long index) => GetParent<KeysCont>().charkey = key;
    }
}
