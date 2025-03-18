using APC;
using APCEvents.Color;
using APCEvents.UI;
using Godot;
using state_types;
using System;

public partial class ColorOptions : VBoxContainer
{
	// Called when the node enters the scene tree for the first time.
	private btn_type _current_type = btn_type.INNER;
	[Export]
	public btn_type btn_Type { get { return _current_type; } set { 
		this._current_type = value;
		this.swap();
		} }
	private string _label = "";
	[Export]
	public Label labelNode { get; set; }

	[Export]
	public string label { get {
			return _label;	
		} set {
			this._label = value;
		} }
	[Export]
	public int index { get; set; } = -1;
	[Export]
	public OptionButton button { get; set; }

	[Export]
	public string Group { get; set; } = "";
	[Export]
	public bool ComplexSelect { get; set; } = false;
    public override void _Ready()
	{
        this.labelNode.Text = this._label;
		if (ComplexSelect) { 

		} else {
			this.button.ItemSelected += (long color) => {
				Bus.Publish<ColorTransitionChangedEvent, ColorTransitionChangedEventArgs>(new() { index = this.index, color = (int)color });
				if (index == 0) {
					Bus.Publish<ImmediateColorChangeEvent, ColorTransitions>(new ColorTransitions() { sequence = new int[1] { (int)color } });
				}
			};
			Bus.Subscribe<BtnSelectedEvent, BtnSelectedEventArgs>((args) => CallDeferred("setEnums"));
			Bus.Subscribe<SetColorEvent, ColorTransitions>(args => {
				if (args is not ComplexColorTransition)
				{
					CallDeferred("setEnums");
				}
			});
		}
		button.AddToGroup(Group);
	}
	public override void _Process(double delta)
	{
	}
	public void setEnums()
	{
		var btn = State.Instance.selected_btn;
		this._current_type = btn.type;
		if (btn.colorTransitions != null)
		{
			int[] sequence = btn.colorTransitions.sequence;
			if ((sequence.Length - 1) >= index) {
				button.Select(sequence[index]);
			} else { 
				button.Select(-1); 
			}
		}
	}
	private void swap()
	{
		bool isOuter = this._current_type == btn_type.OUTER;
        for (int i = 3; i < 7; i++)
        {
			button.SetItemDisabled(i, isOuter);
        }
	}
}
