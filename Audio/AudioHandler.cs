using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APCGear.Audio;
using AudioSwitcher;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;
using AudioSwitcher.AudioApi.Session;
using Godot;

public static class AudioHandler
{
    public static CoreAudioDevice ctrl = new CoreAudioController().DefaultPlaybackDevice;
    public static IEnumerable<IAudioSession> sessions { get { return ctrl.SessionController.ActiveSessions(); } }
    
    public static IEnumerable<Process> processes { get { return sessions.Select(sesh => new Process() { name = sesh.DisplayName, id = sesh.ProcessId}); } }
    public static IEnumerable<string> names { get { return sessions.Select(sesh => sesh.DisplayName); } } //FIXME use text and id together add processes static
    public static IAudioSession? getProcess(Process process) {
        if (names.Where(name => process.name == name).ToList().Count() == 1) { //find by name
            return sessions.ToList().Find(sesh => sesh.DisplayName == process.name); 
        } //find by process_id / equality
        var find = processes.ToList().FindIndex(pro => pro == process);
        if (find == -1) {
            return null;
        } else {
            return sessions.ToList().Find(sesh => sesh.ProcessId == process.id);
        }
    }
    public static void setProcessVolume(Process p, int volume)
    {
        var process = getProcess(p);
        if (process != null)
        {
            process.Volume = volume;
        }
    }
    public static void setMasterVolume(int volume) => ctrl.Volume = volume;
    public static void setMasterMute(bool p) => ctrl.Mute(p);
    public static void setMasterMuteToggle() => ctrl.Mute(!ctrl.IsMuted);
    public static void muteProcess(Process p) 
    {
        var process = getProcess(p);
        if (process != null)
        {
            process.IsMuted = !process.IsMuted;
        }
    }
}