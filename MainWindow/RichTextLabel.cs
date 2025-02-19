using APCEvents.Debug;
using Godot;
using System;

public partial class RichTextLabel : Godot.RichTextLabel
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Bus.Subscribe<DbgEvent, DebugString>(args => CallDeferred("update", args.name, args.args));
	}
	public void update(string name, string args)
	{
        AppendText(name);
        AppendText(args);

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
}
