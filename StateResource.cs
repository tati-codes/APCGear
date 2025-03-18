using APC;
using APCEvents;
using APCEvents.APCIn;
using Godot;
using System;
using System.Collections.Generic;
using state_types;
using APCEvents.KeyAction;
using WindowsInput;
using APCEvents.APCOut;
using APCEvents.Color;
using APCEvents.UI;
using APCGear.Audio;

[GlobalClass][Tool]
public partial class StateResource : Resource
{

    [Export]
    public double version = 0.4;
    [Export]
    public Godot.Collections.Dictionary<int, Godot.Collections.Dictionary<string, int>> slider_table = new Godot.Collections.Dictionary<int, Godot.Collections.Dictionary<string, int>>() { };
    [Export]
    public Godot.Collections.Dictionary<int, Variant> button_table = new Godot.Collections.Dictionary<int, Variant>() { };
    public static readonly string path = "user://config.tres";
    public void sub()
    {
        Bus.Subscribe<ShortcutChanged, ShortcutEventArgs>((args) => this.store_button());
        Bus.Subscribe<SetColorEvent, ColorTransitions>((args) => this.store_button());
        Bus.Subscribe<SetComplexColorEvent, ComplexColorTransition>((args) => this.store_button());
        Bus.Subscribe<SetTextEvent, TextEventArgs>((args) => this.store_button());
        Bus.Subscribe<SetAdvanceOnPress, SetAdvanceOnPressArgs>((args) => this.store_button());
        Bus.Subscribe<ButtonMutesProcess, Process>((args) => this.store_button());
        Bus.Subscribe<LinkSliderToProcess, LinkSliderArgs>(args => store_slider(args.slider));
    }
    public void store_button()
    {
        GD.Print("Storing button");
        var id = State.Instance.selected_id;
        var btn = State.Instance.selected_btn;
        if (button_table.ContainsKey(id))
        {
            button_table[id] = btn.export();
        } else
        {
            button_table.Add(id, btn.export());
        }
        ResourceSaver.Save(this, path);
    }

    public void clear()
    {
        slider_table = new Godot.Collections.Dictionary<int, Godot.Collections.Dictionary<string, int>>() { };
        button_table = new Godot.Collections.Dictionary<int, Variant>() { };
    }

    public void store_slider(int index)
    {
        GD.Print("Storing slider");
        var P = State.Instance.slider_table[(sliders)index].process.export();
        GD.Print(index, P);
        if (slider_table.ContainsKey(index))
        {
            slider_table[index] = P;
        } else
        {
            slider_table.Add(index, P);
        }
        ResourceSaver.Save(this, path);
    }
}