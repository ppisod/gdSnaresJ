using Godot;
using System;

public partial class Slider : MarginContainer {

	public bool @fixed = false;

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
		@fixed = true;
	}

	public int LMarg = 10;
	public int RMarg = 10;
	public int maxHeight = 0;
	public float ERatio = 1f;

	// introduce anim constants
	public const int goalHeight = 50;
	public const double time = 0.5;

	// time
	public double elapsed = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		Visible = false;
		fixMargin();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		elapsed += delta;
		if (elapsed < time)
		{
			maxHeight = (int) Single.Lerp ( 0, goalHeight, (float) elapsed / (float) time );
		}
		else
		{
			maxHeight = goalHeight;
		}
		// use Size vector to determine how much padding to apply
		fixMargin( );
	}
}
