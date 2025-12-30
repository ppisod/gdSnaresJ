using Godot;
using System;
using snaresJ.script.beatmaps.Objects;

public partial class SliderTrack : Control {

	public bool Active = false;

	public Slider slider;

	public Track track;
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
