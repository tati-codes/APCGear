using APCEvents.UI;
using Godot;
using System;

public partial class RootColorControl : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		this.Hide();
		Bus.Subscribe<BtnSelectedEvent, BtnSelectedEventArgs>((args) => this.Show());
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
