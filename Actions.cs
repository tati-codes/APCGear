using APC;
using APCEvents.APCIn;
using state_types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class IAPCArgs { }
public abstract class APCEvent<APCEventArgs> where APCEventArgs : IAPCArgs
{
    private Action<APCEventArgs> _actions = args => { };
    public void Subscribe(Action<APCEventArgs> action) => _actions += action;
    public void Publish(APCEventArgs message) => _actions(message);
}

namespace APCEvents {  

    namespace APCOut
    {

        public class IsConnectedEventArgs : IAPCArgs
        {
            public bool connected { get; set; }
        }

        public class IsConnectedEvent : APCEvent<IsConnectedEventArgs> { }

        public class BtnId : IAPCArgs
        {
            public int Id { get; set; }
        }

        public class BtnPressedEvent : APCEvent<BtnId>
        {

        }

        public class BtnReleasedEvent : APCEvent<BtnId>
        {

        }

        public class SliderChangedEventArgs : IAPCArgs
        {
            public int id { get; set; }
            public int value { get; set; }
        }

        public class SliderChangedEvent : APCEvent<SliderChangedEventArgs>
        {
        }
    }

    namespace UI
{
    public class BtnSelectedEventArgs : IAPCArgs
    {
        public int id { get; set; }
    }

    public class BtnSelectedEvent : APCEvent<BtnSelectedEventArgs>
    {

    }
    public class ChangeColorInUiEvent: APCEvent<BtnSelectedEventArgs> { }

    public class BtnPublishPositionEvent: APCEvent<Position> { }
    public class Position: IAPCArgs
        {
            public Godot.Vector2 pos { get; set; }
        }
}

    namespace APCIn
    {
        public class CycleLedEventArgs : IAPCArgs { 
            public int id { get; set; }
        }

        public class CycleLedEvent : APCEvent<CycleLedEventArgs> { 
        }
    }

    namespace KeyAction
    {
        public class ShortcutEventArgs : IAPCArgs
        {
            public Hotkey Hotkey { get; set; }
        }

        public class ShortcutEvent : APCEvent<ShortcutEventArgs>
        {

        }

        public class TextEventArgs : IAPCArgs
        {
            public string text { get; set; }
        }

        public class TextEvent : APCEvent<TextEventArgs>
        {

        }

        public class SetTextEvent : APCEvent<TextEventArgs> { }

        public class ShortcutChanged : APCEvent<ShortcutEventArgs>
        {

        }

        public class SelectedModsChangedArgs : IAPCArgs
        {
            public Keys new_mods { get; set; }
        }
        public class SelectedModsChanged : APCEvent<SelectedModsChangedArgs> { }

        public class SelectedCharChangedArgs : IAPCArgs { 
            public Keys new_chars { get; set; }  
        }
        public class SelectedCharChanged: APCEvent<SelectedCharChangedArgs> { }
    }
}
