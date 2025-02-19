using APCGear.Audio;
using AudioSwitcher.AudioApi.CoreAudio;
using AudioSwitcher.AudioApi.Session;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class Audio : Node
{
    // Called when the node enters the scene tree for the first time.
    [Export]
    public Timer timer { get; set; }   
    public /* IEnumerable<IAudioSession> */ void GetProcesses()
    {
        //var ctrl = new CoreAudioController();
        //return ctrl.SessionController.ActiveSessions();
    }

    //public List<string> names()
    //{
    //    foreach (IAudioSession session in GetProcesses())
    //    {
    //        GD.Print("session");
    //    }
    //    return new List<string>();
    //}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Ready()
	{
        timer.Timeout += () => Bus.Publish<AudioSessionsRefresh, AudioSessionsRefreshArgs>(new()) ;
	}

}
