using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioSwitcher;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
using AudioSwitcher.AudioApi.Session;
using Godot;

public static class AudioHandler
{
    //FIXME we should keep audio references via text
    public static CoreAudioDevice ctrl = new CoreAudioController().DefaultPlaybackDevice;
    public static IEnumerable<IAudioSession> sessions { get { return ctrl.SessionController.ActiveSessions(); } }
    public static IEnumerable<string> names { get { return sessions.Select(sesh => sesh.DisplayName); } }
    public static IAudioSession? getProcess(int process_id)
    {
        int idx = sessions.ToList().FindIndex(sesh => sesh.ProcessId == process_id);
        if (idx > -1) return sessions.ToList()[idx];
        return null;
    }
    public static void setProcessVolume(int process_id, int volume)
    {
        var process = getProcess(process_id);
        if (process != null)
        {
            process.Volume = volume;
        }
    }
    public static void setMasterVolume(int volume) => ctrl.Volume = volume;
    public static void setMasterMute(bool p) => ctrl.Mute(p);
    public static void setMasterMuteToggle() => ctrl.Mute(!ctrl.IsMuted);
    public static void muteProcess(int process_id)
    {
        var process = getProcess(process_id);
        if (process != null)
        {
            process.IsMuted = !process.IsMuted;
        }
    }
}
public class AudioObserver : IObserver<DevicePropertyChangedArgs>
{
    private IDisposable unsubscriber;
    private string instName;

    public AudioObserver(string name)
    {
        this.instName = name;
    }

    public string Name
    {
        get
        {
            return this.instName;
        }
    }

    public virtual void Subscribe(IObservable<DevicePropertyChangedArgs> provider)
    {
        if (provider != null)
            unsubscriber = provider.Subscribe(this);
    }

    public virtual void OnCompleted()
    {
        GD.Print("The headquarters has completed transmitting data to {0}.", this.Name);
        this.Unsubscribe();
    }

    public virtual void OnError(Exception e)
    {
        GD.Print("{0}: Cannot get message from headquarters.", this.Name);
    }

    public virtual void OnNext(DevicePropertyChangedArgs value)
    {
        GD.Print("{1}: Message I got from headquarters: {0}", value.PropertyName, this.Name);
    }

    public virtual void Unsubscribe()
    {
        unsubscriber.Dispose();
    }
}