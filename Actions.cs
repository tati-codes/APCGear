using APC;
using APCGear.APCIn;
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

namespace APCGear {  

    namespace APCOut
    {

        public class IsConnectedEventArgs : IAPCArgs
        {
            public bool connected { get; set; }
        }

        public class IsConnectedEvent : APCEvent<IsConnectedEventArgs> { }

        public class BtnPressedEventArgs : IAPCArgs
        {
            public int Id { get; set; }
        }

        public class BtnPressedEvent : APCEvent<BtnPressedEventArgs>
        {

        }

        public class BtnReleasedEventArgs : IAPCArgs
        {
            public int Id { get; set; }
        }

        public class BtnReleasedEvent : APCEvent<BtnReleasedEventArgs>
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
}

    namespace APCIn
    {
        public class CycleLedEventArgs : IAPCArgs { 
            public int id { get; set; }
        }

        public class CycleLedEvent : APCEvent<CycleLedEventArgs> { 
        }
    }

    namespace Color
    {
        public enum TransitionType
        {
            CYCLE_ALL,
            CYCLE_SOLIDS,
            CUSTOM
        }
        public class ColorTransitions : IAPCArgs 
        {
            public static ColorTransitions inner_all = new ColorTransitions()
            {
                type = btn_type.INNER,
                sequence = new int[7] { 0, 3, 4, 5, 6, 1, 2 },
                current = 0,
                color_type = TransitionType.CYCLE_ALL
            };
            public static ColorTransitions inner_solids = new ColorTransitions()
            {
                type = btn_type.INNER,
                sequence = new int[4] { 0, 3, 5, 1 },
                current = 0,
                color_type = TransitionType.CYCLE_SOLIDS,
            };
            public static ColorTransitions outer_all = new ColorTransitions()
            {
                type = btn_type.OUTER,
                sequence = new int[3] { 0, 1, 2 },
                current = 0,
                color_type = TransitionType.CYCLE_ALL
            };

            public static ColorTransitions outer_solids = new ColorTransitions()
            {
                type = btn_type.OUTER,
                sequence = new int[2] { 0, 1 },
                current = 0,
                color_type = TransitionType.CYCLE_SOLIDS
            };

            public btn_type type;
            public int[] sequence;
            public int current;
            public TransitionType color_type = TransitionType.CUSTOM;
            public int currentColor { get { return this.sequence[current]; } }
        }
        public class SetColorEvent : APCEvent<ColorTransitions> { }
        public class ColorTransitionChangedEventArgs : IAPCArgs
        {
            public int index;
            public int color;
        }
        public class ColorTransitionChangedEvent: APCEvent<ColorTransitionChangedEventArgs> { }
        public class ImmediateColorChangeEvent: APCEvent<ColorTransitions> { }
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
