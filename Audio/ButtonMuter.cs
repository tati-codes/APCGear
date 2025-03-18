using APC;
using APCEvents.UI;
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
        Bus.Subscribe<BtnSelectedEvent, BtnSelectedEventArgs>(args =>
        {
            if (State.Instance.selected_btn != null && State.Instance.selected_btn.process != Process.nullProcess)
            {
                options.selectedReferece = State.Instance.selected_btn.process;
            } else if (State.Instance.selected_btn != null && State.Instance.selected_btn.process == Process.nullProcess)
            {
                options.selectedReferece = Process.nullProcess;
            }
            options.update();
        });
    }
}
