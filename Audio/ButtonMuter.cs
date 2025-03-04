using APC;
using APCGear.Audio;
using Godot;
using System;

public partial class ButtonMuter : Control
{
    [Export]
    public AudioOptions options { get; set; }
    public override void _Ready()
    {
        options.AudioSelected += (int id, string name) =>
        {
            Bus.Publish<ButtonMutesProcess, ProcessID>(new ProcessID() { process = id });
        };
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
