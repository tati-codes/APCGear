using APCEvents;
using APCEvents.KeyAction;
using APCEvents.UI;
using Godot;
using System;

public partial class ShortcutsPanelMainWindow : MarginContainer
{
	[Export]
	public CharMenu child { get; set; }
	[Export]
	public AddAnother add { get; set; }
    public override void _Ready()
	{
		Bus.Subscribe<BtnSelectedEvent, BtnSelectedEventArgs>(args => CallDeferred("show_selected", args.id));
	}

	public void show_selected(int id)
	{
		var state = State.Instance;
		add.reset();
		child.Select(-1);
		if (id != state.selected_id)
		{
			GD.PrintErr("Weird happenings in KeyPicker scene");
		} else if (state.selected_btn.hotkey.HasValue)
		{
			Hotkey toSlorp = state.selected_btn.hotkey.Value;
			var charSelect = child.GetItemIndex((int)toSlorp.charKey);
			child.Select(charSelect);
			var mods = GetTree().GetNodesInGroup(group: "mods");
            for (global::System.Int32 i = 0; i < toSlorp.mods.Length; i++)
            {
				if (i > 2) throw new Exception("this should never happen?");
				Mods currentNode = mods[i] as Mods;
				var currentMod = toSlorp.mods[i];
				if (currentNode != null)
				{
                    int modSelect = currentNode.GetItemIndex((int)currentMod);
					currentNode.Select(modSelect);
                }
				if (i > 0)
				{
					add._Pressed();
				}
            }
        }
	}

	//public void resolve_event(APCEvents.Action action)
	//{
	//	BtnSelected selected_btn = action as BtnSelected;
	//	if (selected_btn != null) {
	//		CallDeferred("show_selected", selected_btn.payload);
	//	}
	//}
}
