using Godot;
using System;
using snaresJ.script.beatmaps.Objects;

public partial class SliderTrack : Control {

	public bool Active = false;

	public Slider slider;

	public Track track;

	// TODO: public Tracker<Snare> snares;

	public Game gameInstance;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		slider = GetNode <Slider> ( "slider" );
	}

	public override void _Process(double delta)
	{

		// updating the sliderTrack is done here

		if (slider.@fixed)
		{
			SetVisible (true);
		}
	}
}
