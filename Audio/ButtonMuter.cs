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
            Bus.Publish<ButtonMutesProcess, Process>(new Process() { name = name, id = id }); 
        };
    }
}
