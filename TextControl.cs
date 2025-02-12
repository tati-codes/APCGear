using APCGear.KeyAction;
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
	}



    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
