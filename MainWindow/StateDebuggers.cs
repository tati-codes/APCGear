using APC;
using APCEvents.Debug;
using Godot;
using state_types;
using System;
using System.Reflection;

public partial class StateDebuggers : RichTextLabel
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Bus.Subscribe<DbgEvent, DebugString>(args => CallDeferred("update"));
    }

	public void update ()
	{
		this.Text = "";
		if (State.Instance.selected_id > -1)
		{
            AppendText("ID: " + State.Instance.selected_id + "\n");
            var button = State.Instance.selected_btn;
			if (button != null)
			{
				AppendText("TYPE: " + button.type.ToString() + "\n");
                AppendText("ACTION: " + button.action.ToString() + "\n");
				AppendText("ADVANCEONPRESS: " + button.advanceColorOnPress.ToString() + "\n");
				if (button.colorTransitions != null)
				{
					AppendText("currentcolor:" + button.colorTransitions.currentColor + "\n");
				}
				if (button.hotkey != null)
				{
					AppendText("HOTKEY:" + button.hotkey.Value.charKey + " ");
                    foreach (var kv in button.hotkey.Value.mods)
					{
						AppendText(kv + " ");
					}

                }
				if (button.text != null)
				{
					AppendText("TEXT:" + button.text);
				}
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
