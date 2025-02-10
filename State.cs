using APC;
using APCGear;
using APCGear.APCIn;
using APCGear.APCOut;
using APCGear.KeyAction;
using APCGear.UI;
using Godot;
using state_types;
using System;
using System.Collections.Generic;

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
            buttons.Add(i, new state_types.btn_state(state_types.btn_type.INNER, 0, state_types.btn_action.LED_CYCLE));
        }
        foreach (var item in outer)
        {
            buttons.Add((int)(item), new state_types.btn_state(state_types.btn_type.OUTER, 0, state_types.btn_action.LED_CYCLE));
        }
        Bus.Subscribe<BtnSelectedEvent, BtnSelectedEventArgs>((BtnSelectedEventArgs args) => { this.selected_btn = args.id; });
        Bus.Subscribe<BtnReleasedEvent, BtnReleasedEventArgs>((BtnReleasedEventArgs args) => {
            var btn = buttons[args.Id];
            switch (btn.action)
            {
                case state_types.btn_action.LED_CYCLE:
                    Bus.Publish<CycleLedEvent, CycleLedEventArgs>(new CycleLedEventArgs() { id = args.Id });
                    break;
                case state_types.btn_action.SHORTCUT:
                    if (btn.hotkey != null) {
                        Hotkey hotkey = btn.hotkey.Value;
                        Bus.Publish<ShortcutEvent, ShortcutEventArgs>(new ShortcutEventArgs() { Hotkey = hotkey });
                    }
                    break;
                case state_types.btn_action.OBS:
                    break;
                default:
                    break;
            }
        });
        Bus.Subscribe<ShortcutChanged, ShortcutEventArgs>((args) =>
        {
            btn_state state = this.buttons[selected_btn];
            state.hotkey = args.Hotkey;
            state.action = btn_action.SHORTCUT;
            this.buttons[selected_btn] = state;
        });
        Bus.Subscribe<CycleLedEvent, CycleLedEventArgs>(cycleLed);
        Bus.Subscribe<ShortcutEvent, ShortcutEventArgs>(enactShortcut);

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
        LED_CYCLE,
        SHORTCUT,
        OBS,
    }
    public struct btn_state
    {
        public btn_type type;
        public int color;
        public btn_action action;
        public Hotkey? hotkey;
        public btn_state(btn_type t, int c, btn_action a)
        {
            type = t;
            color = c;
            action = a;
            hotkey = null;
        }

    }
}