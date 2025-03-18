using APC;
using Godot;
using state_types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APCEvents
{
    namespace Debug
    {
        public class DebugString: IAPCArgs
        {
            public string name;
            public string args;
        }
        public class DbgEvent : APCEvent<DebugString> { }
    }
    namespace Color
    {
        public enum TransitionType
        {
            CYCLE_SOLIDS,
            TOGGLE,
            CYCLE_ALL,
            CUSTOM_LOOP,
            COMPLEX,
            OBS
        }
        public class ColorTransitions : IAPCArgs
        {
            public static ColorTransitions inner_all { get { return new ColorTransitions()
            {
                type = btn_type.INNER,
                sequence = new int[7] { 0, 3, 4, 5, 6, 1, 2 },
                current = 0,
                color_type = TransitionType.CYCLE_ALL
            }; } }
            public static ColorTransitions inner_solids { get
                {
                    return new ColorTransitions()
                    {
                        type = btn_type.INNER,
                        sequence = new int[4] { 0, 3, 5, 1 },
                        current = 0,
                        color_type = TransitionType.CYCLE_SOLIDS,
                    };
                } }

            public static ColorTransitions outer_all
            {
                get {
                    return new ColorTransitions()
                    {
                        type = btn_type.OUTER,
                        sequence = new int[3] { 0, 1, 2 },
                        current = 0,
                        color_type = TransitionType.CYCLE_ALL
                    };
                }
            }
            public static ColorTransitions outer_solids { get {
                    return new ColorTransitions()
                    {
                        type = btn_type.OUTER,
                        sequence = new int[2] { 0, 1 },
                        current = 0,
                        color_type = TransitionType.CYCLE_SOLIDS
                    };
                } }
            public btn_type type;
            public int[] sequence = new int[0];
            private int current = 0;
            public TransitionType color_type = TransitionType.CUSTOM_LOOP;
            public int currentColor { get {
                    if (sequence.Length == 0) return -1;
                    return this.sequence[current]; } }
            public void advance ()
            {
                int max_length = sequence.Length - 1;
                var seq = sequence;
                if (sequence.Length == 0) { return; }
                else if (current == max_length) {
                    current = 0;
                } else {
                    current += 1;
                }
            }
            public int next()
            {
                advance();
                return currentColor;
            }
            public Godot.Collections.Dictionary export()
            {

                return new Godot.Collections.Dictionary() {
                    { "type", (int)color_type },
                    { "sequence", sequence }
                };
            }

            public static ColorTransitions import(Godot.Collections.Dictionary serial)
            {
                int[] sequence = serial["sequence"].AsInt32Array();
                int colortype = serial["type"].AsInt32();
                return new ColorTransitions()
                {
                    type = btn_type.INNER,
                    sequence = sequence,
                    current = 0,
                    color_type = (TransitionType)colortype
                };
            }
        }
        public class ComplexColorTransition: ColorTransitions
        {
            public Dictionary<inner_state, (inner_state? pressed, inner_state? released)> colors = new();
            public new inner_state currentColor = inner_state.OFF;
            public new TransitionType color_type = TransitionType.COMPLEX;
            public inner_state pressed()
            {
                if (colors.ContainsKey(currentColor) && colors[currentColor].pressed.HasValue)
                {
                    return colors[currentColor].pressed.Value;
                }
                else return currentColor;
            }
            public inner_state released()
            {
                if (colors.ContainsKey(currentColor) && colors[currentColor].released.HasValue)
                {
                    var toReturn = colors[currentColor].released.Value;
                    this.currentColor = toReturn;
                    return toReturn;
                } else return currentColor;
            }
            public new Godot.Collections.Dictionary export()
            {
                Godot.Collections.Array excolors = new Godot.Collections.Array();
                foreach (KeyValuePair<inner_state, (inner_state? pressed, inner_state? released)> item in colors)
                {
                    var key = item.Key;
                    var val = item.Value;
                    var toadd = new Godot.Collections.Dictionary() {
                        { "key", (int)item.Key }};
                    if (item.Value.pressed.HasValue)
                    {
                        toadd.Add("pressed", (int)val.pressed.Value); 
                    } 
                    if (item.Value.released.HasValue)
                    {
                        toadd.Add("released", (int)val.released.Value);
                        excolors.Add(toadd);
                    }
                }
                return new Godot.Collections.Dictionary() {
                    { "type", (int)color_type },
                    { "sequence", excolors }
                };
            }
            public static new ColorTransitions import(Godot.Collections.Dictionary serial)
            {
                var arr = serial["sequence"].AsGodotArray<Godot.Collections.Dictionary>();
                var result = new ComplexColorTransition();
                foreach (var item in arr)
                {
                    inner_state key = (inner_state)item["key"].AsInt32();
                    inner_state? pressed = item.ContainsKey("pressed") ? (inner_state)item["pressed"].AsInt32() : null;
                    inner_state? released = item.ContainsKey("released") ? (inner_state)item["released"].AsInt32() : null;
                    result.colors.Add(key, (pressed: pressed, released: released));
                }
                return result;
            }
        }
        public class SetColorEvent : APCEvent<ColorTransitions> { }
        public class SetComplexColorEvent : APCEvent<ComplexColorTransition> { }
        public class SetAdvanceOnPressArgs : IAPCArgs
        {
            public bool advance;
        }
        public class SetAdvanceOnPress : APCEvent<SetAdvanceOnPressArgs> { }
        public class ColorTransitionChangedEventArgs : IAPCArgs
        {
            public int index;
            public int color;
        }
        public class ComplexColorOptionChosenArgs : IAPCArgs
        {
            public inner_state key;
            public inner_state? pressed;
            public inner_state? released;
        }
        public class ComplexColorTransitionChangedEvent: ComplexColorOptionChosenArgs
        {

        }
        public class ComplexColorOptionChosenEvent : APCEvent<ComplexColorOptionChosenArgs>
        {

        }
        public class ColorTransitionChangedEvent : APCEvent<ColorTransitionChangedEventArgs> { }
        public class ImmediateColorChangeEvent : APCEvent<ColorTransitions> { }
        public class ToggleMiddleRow: APCEvent<Toggle> { }
        public class Toggle : IAPCArgs
        {
            public bool show;
        }

    }
}
