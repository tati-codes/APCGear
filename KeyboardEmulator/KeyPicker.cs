using APCEvents;
using APCEvents.KeyAction;
using Godot;
using state_types;
using System;

public partial class KeyPicker : Control
{
	// Called when the node enters the scene tree for the first time.
	[Export]
	PackedScene charScene { get; set; }
	[Export]
	PackedScene modsScene { get; set; }
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}
