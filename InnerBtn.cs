using Godot;
using System;
[Tool]
public partial class InnerBtn : TextureButton
{
	// Called when the node enters the scene tree for the first time.
    [Export]
    public int id { get; set; } = 0;

    [Signal]
    public delegate void BtnPressedEventHandler(int id);
    
    [Signal]
    public delegate void BtnReleasedEventHandler(int id);

    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
    public override void _Pressed()
    {
        base._Pressed();
        GD.Print(id);
        EmitSignal(nameof(BtnPressed), id);
    }
}
