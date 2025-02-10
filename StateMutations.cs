using APC;
using APCGear;
using APCGear.APCIn;
using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using state_types;
using APCGear.KeyAction;
using WindowsInput;

public partial class State : Node
{
    public void cycleLed(CycleLedEventArgs ev)
    {
        var btn = buttons[ev.id];
        var max_length = Enum.GetValues(btn.type == btn_type.INNER ? typeof(inner_state) : typeof(outer_state)).Length - 1;
        if (btn.color == max_length)
        {
            btn.color = 0;
        } else
        {
            btn.color += 1;
        }
        buttons[ev.id] = btn;
        switch (btn.type)
        {
            case btn_type.INNER:
                Pad.Instance.handler.set_inner(ev.id, (inner_state)btn.color);
                break;
            case btn_type.OUTER:
                Pad.Instance.handler.set_outer((APC.outer_btns)ev.id, (outer_state)btn.color);
                break;
            default:
                break;
        }
    }
    public void enactShortcut(ShortcutEventArgs shortcutEventArgs)
    {
        Keyboard.simulateKeypress(shortcutEventArgs.Hotkey);
    }
}

