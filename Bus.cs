using APCGear;
using APCGear.APCIn;
using Godot;
using System;
using System.Collections.Generic;

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
        presEvent.Subscribe(action);
    }
    public static void Publish<T, APCEventArgs>(APCEventArgs args) where T : APCEvent<APCEventArgs>, new()
        where APCEventArgs : IAPCArgs
    {
        var presEvent = GetEvent<T, APCEventArgs>();
        GD.Print(presEvent);
        presEvent.Publish(args);
    }

    public override void _Ready()
    {
    }
}

