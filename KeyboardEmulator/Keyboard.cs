using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;
using VKC = WindowsInput.Native.VirtualKeyCode;

namespace APCGear.KeyboardEmulator
{
    public struct Hotkey
    {
        public VKC charKey;
        public VKC? mod1;
        public VKC? mod2;
        public VKC? mod3;
        public List<VKC> mods()
        {
            List<VKC> result = new List<VKC>() { };
            if (mod1.HasValue) { result.Add(mod1.Value); }
            if (mod2.HasValue) { result.Add(mod2.Value); }
            if (mod3.HasValue) { result.Add(mod2.Value); }
            return result;
        }
        public Hotkey(VKC charKey, VKC? mod1 = null, VKC? mod2 = null, VKC? mod3 = null)
        {
            this.charKey = charKey;
            this.mod1 = mod1;
            this.mod2 = mod2;
            this.mod3 = mod3;
        }
    }
    public static class Keyboard
    {
        static private InputSimulator simulator = new InputSimulator();
        public static void simulateKeypress(Hotkey action)
        {
            Console.WriteLine($"Pressing {action.charKey} {action.mod1} {action.mod2}");
            if (action.mods().Count < 1)
            {
                simulator.Keyboard.KeyPress(action.charKey);
            }
            else
            {
                simulator.Keyboard.ModifiedKeyStroke(action.mods(), action.charKey);
            }
        }
        public static void pasteText(string text) => simulator.Keyboard.TextEntry(text);
        
    }
}
