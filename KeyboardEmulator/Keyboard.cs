using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;
using VKC = WindowsInput.Native.VirtualKeyCode;

namespace APCGear
{
    public struct Hotkey
    {
        public VKC charKey;
        public VKC[] mods;
        public Hotkey(VKC charKey, params VKC[] mods)
        {
            this.charKey = charKey;
            this.mods = mods;
        }
    }

    public struct Keys
    {
        public readonly string label;
        public readonly VirtualKeyCode value;
        public static List<Keys> charKeys = new List<Keys>() {
            new Keys("NONE", VKC.SEPARATOR),
            new Keys("SPACE", VKC.SPACE),
            new Keys("A", VKC.VK_A),
            new Keys("B", VKC.VK_B),
            new Keys("C", VKC.VK_C),
            new Keys("D", VKC.VK_D),
            new Keys("E", VKC.VK_E),
            new Keys("F", VKC.VK_F),
            new Keys("G", VKC.VK_G),
            new Keys("H", VKC.VK_H),
            new Keys("I", VKC.VK_I),
            new Keys("J", VKC.VK_J),
            new Keys("K", VKC.VK_K),
            new Keys("L", VKC.VK_L),
            new Keys("M", VKC.VK_M),
            new Keys("N", VKC.VK_N),
            new Keys("O", VKC.VK_O),
            new Keys("P", VKC.VK_P),
            new Keys("Q", VKC.VK_Q),
            new Keys("R", VKC.VK_R),
            new Keys("S", VKC.VK_S),
            new Keys("T", VKC.VK_T),
            new Keys("U", VKC.VK_U),
            new Keys("V", VKC.VK_V),
            new Keys("W", VKC.VK_W),
            new Keys("X", VKC.VK_X),
            new Keys("Y", VKC.VK_Y),
            new Keys("Z", VKC.VK_Z),
            new Keys("0", VKC.VK_0),
            new Keys("1", VKC.VK_1),
            new Keys("2", VKC.VK_2),
            new Keys("3", VKC.VK_3),
            new Keys("4", VKC.VK_4),
            new Keys("5", VKC.VK_5),
            new Keys("6", VKC.VK_6),
            new Keys("7", VKC.VK_7),
            new Keys("8", VKC.VK_8),
            new Keys("9", VKC.VK_9),
            new Keys("UP", VKC.UP),
            new Keys("DOWN", VKC.DOWN),
            new Keys("RIGHT", VKC.RIGHT),
            new Keys("LEFT", VKC.LEFT),
            new Keys("MUTE", VKC.VOLUME_MUTE),
            new Keys("VOLUME DOWN", VKC.VOLUME_DOWN),
            new Keys("VOLUME UP", VKC.VOLUME_UP),
            new Keys("PLAY/PAUSE", VKC.MEDIA_PLAY_PAUSE),
            new Keys("STOP", VKC.MEDIA_STOP),
            new Keys("NEXT", VKC.MEDIA_NEXT_TRACK),
            new Keys("PREV", VKC.MEDIA_PREV_TRACK),
            new Keys("TAB", VKC.TAB),
            new Keys("CTRL", VKC.CONTROL),
            new Keys("WIN", VKC.RWIN),
            new Keys("ALT", VKC.MENU),
            new Keys("SHIFT", VKC.SHIFT),
        };
        public static List<Keys> modKeys = new List<Keys>(){
            new Keys("NONE", VKC.SEPARATOR),
            new Keys("CTRL", VKC.CONTROL),
            new Keys("WIN", VKC.RWIN),
            new Keys("ALT", VKC.MENU),
            new Keys("SHIFT", VKC.SHIFT),
        };

        public Keys(string label, VirtualKeyCode value)
        {
            this.label = label;
            this.value = value;
        }

        public string toString()
        {
            return this.label;
        }
    }

public static class Keyboard
    {

        static private InputSimulator simulator = new InputSimulator();
        public static void simulateKeypress(Hotkey action)
        {
            GD.Print(action.charKey.ToString(), " ", action.mods.ToString());
            if (action.mods.Length < 1)
            {
                simulator.Keyboard.KeyPress(action.charKey);
            }
            else
            {
                simulator.Keyboard.ModifiedKeyStroke(action.mods, action.charKey);
            }
        }
        public static void pasteText(string text) => simulator.Keyboard.TextEntry(text);
        
    }
}
