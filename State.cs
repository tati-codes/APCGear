using APC;
using APCEvents;
using APCEvents.APCIn;
using APCEvents.APCOut;
using APCEvents.Color;
using APCEvents.KeyAction;
using APCEvents.UI;
using APCGear.Audio;
using Godot;
using state_types;
using System;


public partial class State : Node
{
    public double version = 0.4;
    public StateResource state_resource;
    public static State Instance { get; private set; }
    public int selected_id = -1;

    public Godot.Collections.Dictionary<sliders, slider_state> slider_table = new();
    public Godot.Collections.Dictionary<int, state_types.btn_state> button_table = new();

    public btn_state? selected_btn { get {
            if (selected_id == -1) return null;
            return button_table[selected_id]; } }
    public bool connected_to_apc = false; //FIXME
    public bool connected_to_obs = false;
    public string obs_id = string.Empty;
    public string obs_passwordd = string.Empty;

    //TODO check for previous configs
    //TODO do we need a slider state?
    public override void _Ready()
    {
        //TODO autoassign
        //TODO store_button slider stuff
        Instance = this;
        if (FileAccess.FileExists(StateResource.path))
        {
            state_resource = ResourceLoader.Load<StateResource>(StateResource.path);
            GD.Print(state_resource.version, version);
            if (state_resource.version != version)
            {
                state_resource.clear();
                GD.Print("clearing");
            }
        } else
        {
            GD.Print("making");
            state_resource = ResourceLoader.Load<StateResource>("res://config/state.tres");
        }

        generate_initial_state();

        Bus.Subscribe<IsConnectedEvent, IsConnectedEventArgs>(args =>
        {
            connected_to_apc = args.connected;
            if (connected_to_apc)
            {
                remember();
            }
        });
        Bus.Subscribe<BtnSelectedEvent, BtnSelectedEventArgs>((BtnSelectedEventArgs args) => { 
            this.selected_id = args.id;
        });
        Bus.Subscribe<BtnReleasedEvent, BtnId>((BtnId args) => {
            var btn = button_table[args.Id];
            GD.Print(btn.action);
            //FIXME just do the actions here why are we firing and immediately subscribing to events?
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
                case state_types.btn_action.MUTE:
                    if (btn.process != Process.nullProcess) {
                        mute(btn.process);
                    }
                    break;
                default:
                    break;
            }
        });
        Bus.Subscribe<BtnPressedEvent, BtnId>(args =>
        {
            var currentbtn = button_table[args.Id];
            if (currentbtn.advanceColorOnPress && currentbtn.colorTransitions is ComplexColorTransition) advanceComplexOnPressed(args);
        });

        Bus.Subscribe<ShortcutChanged, ShortcutEventArgs>((args) =>
        {
            btn_state state = this.button_table[selected_id];
            state.hotkey = args.Hotkey;
            state.action = btn_action.SHORTCUT;
            this.button_table[selected_id] = state;
        });
        Bus.Subscribe<SetColorEvent, ColorTransitions>((args) =>
        {
            btn_state state = this.button_table[selected_id];
            state.colorTransitions = args;
            state.colorTransitionIsComplex = false;
            this.button_table[selected_id] = state;
        });
        Bus.Subscribe<SetComplexColorEvent, ComplexColorTransition>((args) =>
        {
            btn_state state = this.button_table[selected_id];
            state.colorTransitions = args;
            state.colorTransitionIsComplex = true;
            this.button_table[selected_id] = state;
        });
        Bus.Subscribe<SetTextEvent, TextEventArgs>((args) =>
        {
            if (selected_btn == null) return;
            btn_state state = this.button_table[selected_id];
            state.text = args.text;
            state.action = btn_action.TEXT;
            this.button_table[selected_id] = state;
        });
        Bus.Subscribe<SetAdvanceOnPress, SetAdvanceOnPressArgs>((args) =>
        {
            if (selected_btn != null)
            {
                btn_state state = this.button_table[selected_id];
                state.advanceColorOnPress = args.advance;
                this.button_table[selected_id] = state;
            }
        });
        Bus.Subscribe<ButtonMutesProcess, APCGear.Audio.Process>((args) =>
        {
            if (selected_btn != null)
            {
                btn_state state = this.button_table[selected_id];
                state.action = btn_action.MUTE;
                state.process = args;
                this.button_table[selected_id] = state;
            }
        });

        Bus.Subscribe<LinkSliderToProcess, LinkSliderArgs>((args) => {
            var slider_tochange = slider_table[(sliders)args.slider];
            slider_tochange.process = args.process;
            slider_table[(sliders)args.slider] = slider_tochange;
        });

        Bus.Subscribe<SliderChangedEvent, SliderChangedEventArgs>((args) =>
        {
            var current = slider_table[(sliders)args.id];
            if (current.process.name != "nullProcess") changeVolume(args);
        });
        Bus.Subscribe<BtnReleasedEvent, BtnId>(advanceColor);
        Bus.Subscribe<ShortcutEvent, ShortcutEventArgs>(enactShortcut);
        Bus.Subscribe<TextEvent, TextEventArgs>(pasteText);
        Bus.Subscribe<ImmediateColorChangeEvent, ColorTransitions>(setButtonColor);
        state_resource.sub(); //it's here deliberately after the subscriptions so the persistence calls save after being modified


    }

    public void generate_initial_state()
    {
        var outer = Enum.GetValues<outer_btns>();
        for (int i = 0; i < 64; i++)
        {
            button_table.Add(i, new state_types.btn_state(state_types.btn_type.INNER, state_types.btn_action.NONE));
        }
        foreach (var item in outer)
        {
            button_table.Add((int)(item), new state_types.btn_state(state_types.btn_type.OUTER, state_types.btn_action.NONE));
        }
        foreach (sliders slider in Enum.GetValues(typeof(sliders)))
        {
            slider_table.Add(slider, new slider_state());
        }
    }
    public void remember() //loads persisted data
    {
        foreach ((int key, var val) in state_resource.slider_table)
        {
            slider_table[(sliders)key] = new slider_state() { process = Process.import(val) };
        }
        foreach ((int key, var val) in state_resource.button_table)
        {
            var p = btn_state.import(val.AsGodotDictionary<string, Variant>());
            button_table[key] = p;
            Bus.Publish<BtnSelectedEvent, BtnSelectedEventArgs>(new BtnSelectedEventArgs() { id = key});
            Bus.Publish<ImmediateColorChangeEvent, ColorTransitions>(p.colorTransitions);
        }
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
        MUTE,
    }
    public partial class btn_state : GodotObject
    {
        public btn_type type;
        public btn_action action;
        public bool advanceColorOnPress = false;
        public Hotkey? hotkey;
        public ColorTransitions? colorTransitions;
        public bool colorTransitionIsComplex = false;
        public string? label = null;
        public string? text = null;
        public APCGear.Audio.Process process = APCGear.Audio.Process.nullProcess;
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
        public Godot.Collections.Dictionary<string, Variant> export()
        {
            var transition = colorTransitionIsComplex ? ((ComplexColorTransition)colorTransitions).export() : colorTransitions.export();
            return new Godot.Collections.Dictionary<string, Variant>()
                    {
                        { "type", (int)type },
                        { "action", (int)action },
                        { "advanceColorOnPress", advanceColorOnPress }, // Vector2 is not supported by JSON
                        { "hotkey", hotkey?.export()},
                        { "label", label},
                        { "text", text },
                        { "colorTransitionIsComplex", colorTransitionIsComplex},
                        { "colorTransitions", transition },
                        { "process", process?.export() } 
                    };
        }

        //TODO import
        public static btn_state import(Godot.Collections.Dictionary<string, Variant> serial)
        {
            try
            {
                int t = serial["type"].AsInt32();
                int a = serial["action"].AsInt32();
                bool advance = serial["advanceColorOnPress"].AsBool();
                bool colorTransitionIsComplex = serial["colorTransitionIsComplex"].AsBool();
                Godot.Collections.Dictionary<string, int>? process = serial["process"].AsGodotDictionary<string, int>();
                string label = serial["label"].AsString(); 
                string text = serial["text"].AsString();
                int transitionType = serial["colorTransitions"].AsGodotDictionary()["type"].AsInt32();
                Godot.Collections.Dictionary hotkey = serial["hotkey"].AsGodotDictionary();
                ColorTransitions transition = (TransitionType)transitionType != TransitionType.COMPLEX ?
                    ColorTransitions.import(serial["colorTransitions"].AsGodotDictionary()) :
                    ComplexColorTransition.import(serial["colorTransitions"].AsGodotDictionary());
                var result = new btn_state((btn_type)t, (btn_action)a) {
                    advanceColorOnPress = advance,
                    colorTransitionIsComplex = colorTransitionIsComplex,
                    process = APCGear.Audio.Process.import(process), 
                    label = label == "" ? null : label,
                    text = text == "" ? null : text,
                    colorTransitions = transition,
                    hotkey = hotkey.Count > 0 ? Hotkey.import(hotkey) : null
                };
                return result;
            } catch (Exception e) {
                GD.Print(e);
                throw new Exception("fucked up importing state");
            }
        }
    }
    public partial class slider_state : GodotObject
    {
        public APCGear.Audio.Process process = Process.nullProcess; 
    }
}