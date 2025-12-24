using Godot;
using System;

public partial class Slider : MarginContainer {

	public void fixMargin ( ) {
		float heightOfCont = Size.Y;
		float paddingToApply = ( heightOfCont - maxHeight ) / 2;
		if (paddingToApply < 0)
		{
			paddingToApply = 0;
		}
		AddThemeConstantOverride ( "margin_top", (int) paddingToApply );
		AddThemeConstantOverride ( "margin_bottom", (int) paddingToApply );
		Visible = true;
	}

	public int LMarg = 10;
	public int RMarg = 10;
	public int maxHeight = 50;
	public float ERatio = 1f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		Visible = false;
		fixMargin();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// use Size vector to determine how much padding to apply
		fixMargin( );
	}
}
