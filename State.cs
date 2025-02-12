using APC;
using APCGear;
using APCGear.APCIn;
using APCGear.APCOut;
using APCGear.Color;
using APCGear.KeyAction;
using APCGear.UI;
using Godot;
using state_types;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

public partial class State : Node
{
    public static State Instance { get; private set; }
    public int selected_btn = -1;
    public btn_state? selected { get { 
            if (selected_btn == -1) return null;
            return buttons[selected_btn]; } }
    public bool connected_to_apc = false;
    public bool connected_to_obs = false;
    public string obs_id = string.Empty;
    public string obs_passwordd = string.Empty;
    public Dictionary<int, state_types.btn_state> buttons = new Dictionary<int, state_types.btn_state>() { };
    //TODO check for previous configs
    //TODO do we need a slider state?
    public override void _Ready()
    {
        Instance = this;
        var outer = Enum.GetValues<outer_btns>();
        for (int i = 0; i < 64; i++)
        {
            buttons.Add(i, new state_types.btn_state(state_types.btn_type.INNER, state_types.btn_action.NONE));
        }
        foreach (var item in outer)
        {
            buttons.Add((int)(item), new state_types.btn_state(state_types.btn_type.OUTER, state_types.btn_action.NONE));
        }
        Bus.Subscribe<BtnSelectedEvent, BtnSelectedEventArgs>((BtnSelectedEventArgs args) => { this.selected_btn = args.id; });
        Bus.Subscribe<BtnReleasedEvent, BtnReleasedEventArgs>((BtnReleasedEventArgs args) => {
            var btn = buttons[args.Id];
            switch (btn.action)
            {
                case state_types.btn_action.SHORTCUT:
                    if (btn.hotkey != null) {
                        Hotkey hotkey = btn.hotkey.Value;
                        Bus.Publish<ShortcutEvent, ShortcutEventArgs>(new ShortcutEventArgs() { Hotkey = hotkey });
                    }
                    break;
                case state_types.btn_action.OBS:
                    break;

                case state_types.btn_action.TEXT:
                    if (btn.text != null)
                    {
                        Bus.Publish<TextEvent, TextEventArgs>(new() { text = btn.text });
                    }
                    break;
                default:
                    break;
            }
        });
        Bus.Subscribe<BtnReleasedEvent, BtnReleasedEventArgs>(advanceColor);
        Bus.Subscribe<ShortcutChanged, ShortcutEventArgs>((args) =>
        {
            btn_state state = this.buttons[selected_btn];
            state.hotkey = args.Hotkey;
            state.action = btn_action.SHORTCUT;
            this.buttons[selected_btn] = state;
        });
        Bus.Subscribe<SetColorEvent, ColorTransitions>((args) =>
        {
            btn_state state = this.buttons[selected_btn];
            state.colorTransitions = args;
            this.buttons[selected_btn] = state;
        });
        Bus.Subscribe<ShortcutEvent, ShortcutEventArgs>(enactShortcut);
        Bus.Subscribe<TextEvent, TextEventArgs>(pasteText);
        Bus.Subscribe<SetTextEvent, TextEventArgs>((args) =>
        {
            btn_state state = this.buttons[selected_btn];
            state.text = args.text;
            state.action = btn_action.TEXT;
            this.buttons[selected_btn] = state;
        });
        Bus.Subscribe<ImmediateColorChangeEvent, ColorTransitions>(setButtonColor);
    }
}

namespace state_types
{
    public enum btn_type
    {
        INNER,
        OUTER
    }
    public enum btn_action
    {
        SHORTCUT,
        OBS,
        TEXT,
        NONE,
    }
    public class btn_state
    {
        public btn_type type;
        public btn_action action;
        public Hotkey? hotkey;
        public ColorTransitions? colorTransitions;
        public string? label;
        public string? text;
        public btn_state(btn_type t, btn_action a)
        {
            type = t;
            action = a;
            hotkey = null;
            switch (t)
            {
                case btn_type.INNER:
                    colorTransitions = ColorTransitions.inner_solids;
                    break;
                case btn_type.OUTER:
                    colorTransitions = ColorTransitions.outer_solids;
                    break;
                default:
                    break;
            }
        }

    }
}