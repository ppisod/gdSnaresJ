using Godot;
using System;

public partial class SliderTrack : Control {

	public Slider slider;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		slider = GetNode <Slider> ( "slider" );
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (slider.@fixed)
		{
			SetVisible (true);
		}
	}
}
