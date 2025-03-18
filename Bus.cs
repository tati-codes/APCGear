using APCEvents;
using APCEvents.APCIn;
using APCEvents.Debug;
using APCGear.Audio;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

public partial class Bus : Node
{

    private static readonly Dictionary<Type, Func<Object>> _mapping = new Dictionary<Type, Func<Object>>() {
        //[typeof(CycleLedEvent)] = () => CycleLedEvent.instance,
    };
    //public static Dictionary<procedural, Func<Object>> emitter = {};
    private static T GetEvent<T, APCEventArgs>()
        where T : APCEvent<APCEventArgs>, new()
        where APCEventArgs : IAPCArgs
    {
        if (_mapping.ContainsKey(typeof(T)))
        {
            return _mapping[typeof(T)]() as T;
        }

        var presEvent = new T();
        _mapping.Add(typeof(T), () => presEvent);

        return presEvent;
    }

    public static void Subscribe<T, APCEventArgs>(Action<APCEventArgs> action) where T : APCEvent<APCEventArgs>, new()
        where APCEventArgs : IAPCArgs
    {
        var presEvent = GetEvent<T, APCEventArgs>();
        //GD.Print(presEvent);
        presEvent.Subscribe(action);
    }
    public static string debugStringify(Object t) {
        Type myClassType = t.GetType();
        PropertyInfo[] properties = myClassType.GetProperties();
        if (myClassType.ToString() == "APCGear.Audio.AudioSessionsRefreshArgs")
        {
            return "";
        }
        string result = "{ \n";
        foreach (PropertyInfo property in properties)
        {
            result += property.Name + ": " + property.GetValue(t, null) + "\n";
        }
        result += "}\n";
        GD.Print(result);
        return result;
    }
    public static void Publish<T, APCEventArgs>(APCEventArgs args) where T : APCEvent<APCEventArgs>, new()
        where APCEventArgs : IAPCArgs
    {
        var presEvent = GetEvent<T, APCEventArgs>();
        var debugEvent = GetEvent<APCEvents.Debug.DbgEvent, DebugString>();
        if (presEvent.ToString() != "APCGear.Audio.AudioSessionsRefresh") GD.Print("Firing: ", presEvent);
        presEvent.Publish(args);
        debugEvent.Publish(new DebugString()
        {
            name = presEvent.ToString(),
            args = debugStringify(args)
        });
    }

    public override void _Ready()
    {
    }
}

