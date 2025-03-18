using APCEvents.UI;
using Godot;
using System;

public partial class FunctionPanel : TabContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
       Bus.Subscribe<BtnSelectedEvent, BtnSelectedEventArgs>((args) => {
		   var btn = State.Instance.selected_btn;
		   if (btn != null)
		   {
			   switch (btn.action)
			   {
					case state_types.btn_action.SHORTCUT: {
						this.CurrentTab = 2;
						break;
					}
					case state_types.btn_action.TEXT: {
						this.CurrentTab = 0;
						break;
					}
				   case state_types.btn_action.MUTE:
					   {
						   this.CurrentTab = 1;
						   break;
					   }
                   case state_types.btn_action.NONE:
                       {
						   this.CurrentTab = -1;
                           break;
                       }
                   default:
					   {
						   break;
					   }
			   }
		   }
		});
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
