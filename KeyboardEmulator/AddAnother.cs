using Godot;
using System;

public partial class AddAnother : Button
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	Mods mods1 { get; set; } 
	[Export]
	Mods mods2 { get; set; } 
	
	int timesPressed = 0;
    public override void _Pressed()
    {
		if (timesPressed == 0)
		{
			mods1.Show();
			timesPressed = 1;
		} else if (timesPressed == 1) {
			mods2.Show();
			Hide();
			timesPressed = 2;
		} 
    }
	public void reset() { 
		timesPressed = 0;
		Show();
		mods1.Hide(); 
		mods2.Hide();
        var mods = GetTree().GetNodesInGroup(group: "mods");
        foreach (var item in mods)
        {
			Mods mod = item as Mods;
			if (mod != null) mod.Select(-1);
        }
    }
}
