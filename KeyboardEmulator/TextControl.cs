using APCEvents.KeyAction;
using APCEvents.UI;
using Godot;
using System;

public partial class TextControl : Control
{
    // Called when the node enters the scene tree for the first time.ExportAttribute
    [Export]
    public Button button { get; set; }
    [Export]
    public TextEdit edit { get; set; }

    public override void _Ready()
    {
        this.button.Pressed += () =>
        {
            Bus.Publish<SetTextEvent, TextEventArgs>(new() { text = edit.Text });
        };
        Bus.Subscribe<BtnSelectedEvent, BtnSelectedEventArgs>(args => CallDeferred("show_selected", args.id));
    }

    public void show_selected(int id)
    {
        edit.Clear();
        var state = State.Instance;
        if (id != state.selected_id)
        {
            GD.PrintErr("Weird happenings in KeyPicker scene");
        }
        else if (state.selected_btn.text != null && state.selected_btn.text != "")
        {
            edit.Text = state.selected_btn.text;
        }
    }
}
