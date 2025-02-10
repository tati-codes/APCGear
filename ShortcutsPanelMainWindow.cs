using APCGear.KeyAction;
using APCGear.UI;
using Godot;
using System;

public partial class ShortcutsPanelMainWindow : MarginContainer
{
	// Called when the node enters the scene tree for the first time.
	private int currentSelected = -1;
	[Export]
	public PackedScene child = null;
	public override void _Ready()
	{
		Bus.Subscribe<BtnSelectedEvent, BtnSelectedEventArgs>(show_selected);
	}

	public void clear_children()
	{
		var children = this.GetChildren();
		foreach (var child in children)
		{
			child.QueueFree();
		}
	}

	public void show_selected(BtnSelectedEventArgs args)
	{
		var state = State.Instance;
		if (args.id != currentSelected)
		{
			currentSelected = args.id;
			var btn = state.buttons[args.id];
			CallDeferred("clear_children");
			KeyPicker node = child.Instantiate<KeyPicker>();
			if (btn.action == state_types.btn_action.SHORTCUT && btn.hotkey != null)
			{
				node.populate((APCGear.Hotkey)btn.hotkey);
			}
			CallDeferred("add_child", node);
		}
	}

	//public void resolve_event(APCGear.Action action)
	//{
	//	BtnSelected selected = action as BtnSelected;
	//	if (selected != null) {
	//		CallDeferred("show_selected", selected.payload);
	//	}
	//}
}
