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
using APCGear.APCOut;
using APCGear.Color;
using APCGear.UI;

public partial class State : Node
{
    public void enactShortcut(ShortcutEventArgs shortcutEventArgs)
    {
        Keyboard.simulateKeypress(shortcutEventArgs.Hotkey);
    }
    public void advanceColor(BtnReleasedEventArgs args)
    {
        btn_state btn = buttons[args.Id];
        if (btn.colorTransitions == null) return;
        int max_length = btn.colorTransitions.sequence.Length - 1;
        var seq = btn.colorTransitions.sequence;
        int current = btn.colorTransitions.current;
        if (current == max_length)
        {
            btn.colorTransitions.current = 0;
        }
        else
        {
            btn.colorTransitions.current += 1;
        }
        buttons[args.Id] = btn;
        switch (btn.type)
        {
            case btn_type.INNER:
                Pad.Instance.handler.set_inner(args.Id, (inner_state)btn.colorTransitions.currentColor);
                break;
            case btn_type.OUTER:
                Pad.Instance.handler.set_outer((APC.outer_btns)args.Id, (outer_state)btn.colorTransitions.currentColor);
                break;
            default:
                break;
        }
        Bus.Publish<ChangeColorInUiEvent, BtnSelectedEventArgs>(new BtnSelectedEventArgs() { id = args.Id});
    }
    public void pasteText(TextEventArgs args) => Keyboard.pasteText(args.text);
    public void setButtonColor(ColorTransitions transitions)
    {
        btn_state btn = selected;
        switch (btn.type)
        {
            case btn_type.INNER:
                Pad.Instance.handler.set_inner(selected_btn, (inner_state)transitions.currentColor);
                break;
            case btn_type.OUTER:
                Pad.Instance.handler.set_outer((APC.outer_btns)selected_btn, (outer_state)transitions.currentColor);
                break;
            default:
                break;
        }
        Bus.Publish<ChangeColorInUiEvent, BtnSelectedEventArgs>(new BtnSelectedEventArgs() { id = selected_btn });
    }
}

