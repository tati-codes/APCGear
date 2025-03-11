using APC;
using APCEvents;
using APCEvents.APCIn;
using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using state_types;
using APCEvents.KeyAction;
using WindowsInput;
using APCEvents.APCOut;
using APCEvents.Color;
using APCEvents.UI;
using APCGear.Audio;

public partial class State : Node
{
    public void enactShortcut(ShortcutEventArgs shortcutEventArgs)
    {
        Keyboard.simulateKeypress(shortcutEventArgs.Hotkey);
    }
    public void advanceColor(BtnId args)
    {
        btn_state btn = button_table[args.Id];
        if (btn.colorTransitions == null) return;
        if (btn.colorTransitions is ComplexColorTransition)
        {
            advanceComplexOnRelease(args);
            return;
        }
        var color = btn.colorTransitions.next();
        switch (btn.type)
        {
            case btn_type.INNER:
                Pad.Instance.handler.set_inner(args.Id, (inner_state)color);
                break;
            case btn_type.OUTER:
                Pad.Instance.handler.set_outer((APC.outer_btns)args.Id, (outer_state)color);
                break;
            default:
                break;
        }
        Bus.Publish<ChangeColorInUiEvent, BtnSelectedEventArgs>(new BtnSelectedEventArgs() { id = args.Id});
    }
    public void pasteText(TextEventArgs args) => Keyboard.pasteText(args.text);
    public void setButtonColor(ColorTransitions transitions)
    {
        btn_state btn = selected_btn;
        switch (btn.type)
        {
            case btn_type.INNER:
                Pad.Instance.handler.set_inner(selected_id, (inner_state)transitions.currentColor);
                break;
            case btn_type.OUTER:
                Pad.Instance.handler.set_outer((APC.outer_btns)selected_id, (outer_state)transitions.currentColor);
                break;
            default:
                break;
        }
        Bus.Publish<ChangeColorInUiEvent, BtnSelectedEventArgs>(new BtnSelectedEventArgs() { id = selected_id });
    }
    public void advanceComplexOnRelease(BtnId args)
    {
        btn_state btn = button_table[args.Id];
        ComplexColorTransition transition = btn.colorTransitions as ComplexColorTransition;
        if (transition != null)
        {
            Pad.Instance.handler.set_inner(selected_id, transition.released());
        }
    }
    public void advanceComplexOnPressed(BtnId args) {
        btn_state btn = button_table[args.Id];
        ComplexColorTransition transition = btn.colorTransitions as ComplexColorTransition;
        if (transition != null)
        {
            Pad.Instance.handler.set_inner(selected_id, transition.pressed());
        }
    }
    public void changeVolume(SliderChangedEventArgs args)
    {
        sliders slid = (sliders)args.id;
        var current = slider_table[slid];
        if (current.process == Process.nullProcess) { 
            return; 
        } else if (current.process.id == 1) {
            AudioHandler.setMasterVolume(args.value);   
        } else {
            AudioHandler.setProcessVolume(current.process, args.value);
        }
    }
    public void mute(Process p)
    {
        if (p == Process.nullProcess) { return; }
        GD.Print("Muting ", p.name);
        if (p.id == 1)
        {
            AudioHandler.setMasterMuteToggle();
        } else 
        {
            AudioHandler.muteProcess(p);
        }
    }
}

