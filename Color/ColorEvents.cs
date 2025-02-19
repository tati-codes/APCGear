using APC;
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
            public int[] sequence;
            private int current;
            public TransitionType color_type = TransitionType.CUSTOM_LOOP;
            public int currentColor { get { return this.sequence[current]; } }
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
        }
        public class ComplexColorTransition: ColorTransitions
        {
            public Dictionary<inner_state, (inner_state? pressed, inner_state? released)> colors = new();

            public new inner_state currentColor = inner_state.OFF;
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
        }
        public class SetColorEvent : APCEvent<ColorTransitions> { }
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
