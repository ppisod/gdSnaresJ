using Godot;
using System;
using System.Collections.Generic;
using snaresJ.script.beatmaps.Objects;

public partial class SliderContainer : Control
{
	public List<SliderTrack> sliders = [];
	public List<int> sliderIds = [];

	private PackedScene sliderTrackScene;
	private VBoxContainer sliderTrackContainer;
	private bool initialized = false;

	/// <summary>
	/// adds a track to the scene
	/// </summary>
	/// <param name="track">the track object to be added</param>
	/// <returns>whether the track was added or not</returns>
	public bool AddTrackToScene ( Track track ) {
		if (!initialized) return false;
		foreach (SliderTrack slider in sliders)
		{
			if (slider.track == track)
			{
				// already exists
				return false;
			}
		}
		if (sliderIds.Contains(track.id))
		{
			return false;
		}

		var sliderTrack = sliderTrackScene.Instantiate <SliderTrack> ();
		sliderTrack.track = track;
		sliderTrackContainer.AddChild (sliderTrack);
		sliders.Add (sliderTrack);
		sliderIds.Add (track.id);

		return true;
	}

	public bool AddTrackToScene ( Track track, bool debug ) {
		if (!initialized) return false;
		foreach (SliderTrack slider in sliders)
		{
			if (slider.track == track)
			{
				// already exists
				if (debug) GD.PrintErr(track.id + ") Track already exists in scene! because track object matches");
				return false;
			}
		}
		if (sliderIds.Contains(track.id))
		{
			if (debug) GD.PrintErr(track.id + ") Track already exists in scene! because slider ids contains track id");
			return false;
		}

		var sliderTrack = sliderTrackScene.Instantiate <SliderTrack> ();
		sliderTrack.track = track;
		sliderTrackContainer.AddChild (sliderTrack);
		sliders.Add (sliderTrack);
		sliderIds.Add (track.id);

		if (debug) GD.Print(track.id + ") Added track to scene");
		return true;
	}

	private void LoadPackedScenes ( ) {
		sliderTrackScene = (PackedScene) ResourceLoader.Load("res://components/sliderTrack.tscn");
	}

	private void GetChildren ( ) {
		sliderTrackContainer = GetNode<VBoxContainer> ("sliders");
	}

	public override void _Ready()
	{
		GetChildren();
		LoadPackedScenes();
		initialized = true;
	}

	public override void _Process(double delta) {
		if (!initialized) return;
		// ...
	}
}
