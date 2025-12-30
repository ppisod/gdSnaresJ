using Godot;
using System;
using snaresJ.script.beatmaps;
using snaresJ.script.beatmaps.Events;
using snaresJ.script.scene;
using snaresJ.script.state;
using snaresJ.script.utility.Rhythm;

public partial class Game : Control {
	private SlipButton back;
	private SliderContainer sliders;

	private double timeInScene;

	private Beatmap bm;
	private EventCollection BeatmapEvents;

	private Metronome metronome;
	private bool initialized = false;

	private bool playing = false;
	private AudioStream songAudio;
	private AudioStreamPlayer audioPlayer;

	public int countInBeats;
	public int trueBeats = 0;
	public double startDelay = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		initializeChildren ();

		// ToBeRemoved
		GD.Print ( State.instance.selectedBeatmap.beatmapPath );
		bm = State.instance.selectedBeatmap;

		// load events
		bm.LoadEvents();

		// this is when the BPM is correctly set
		initializeMetronome ();

		BeatmapEvents = bm.BeatmapEvents;
		BeatmapEvents.SortEventsIntoCategories();

		// load the music
		initializeAudio ();

		// ...
		PreProcessEvents ();

		// gets rid of exceptions
		initialized = true;
	}

	public override void _Process(double delta)
	{
		if (!initialized)
		{
			return;
		}

		// update time
		timeInScene += delta;
		var beats = metronome.TotalBeats;

		if (startDelay > 0)
		{
			startDelay -= delta;
			return;
		}

		if (startDelay < 0)
		{
			startDelay = 0;
			// reset metronome to ensure sync?
			metronome.Stop ();
			metronome.Reset ();
			metronome.Start ();
		}

		if (!playing)
		{
			if (beats == countInBeats) // TODO: this is a buggy check
			{
				playing = true;
				PlaySequence ();
			}
			return;
		}
		// Process Events
		ProcessEvents();
	}

	public void CheckForInitialEvents ( ) {
		foreach (TimelyEvent te in bm.BeatmapEvents.events)
		{
			if (te is IntroduceTrack e && Math.Abs ( e.beat - 0f ) <= countInBeats)
			{
				sliders.AddTrackToScene ( e.GetTrackObject () );
			}
			// add other initial event(s)' pre here.
		}
	}

	public void PlaySequence ( ) {
		audioPlayer.Play ();
		// initial startDelay
		startDelay = bm.startMs;
	}

	public void ProcessEvents ( ) {
		var currentBeatD = metronome.GetCurrentTotalBeats ();
		foreach (TimelyEvent sceneEvent in BeatmapEvents.sceneEvents.GetNext ( BeatmapEvents.sceneEventPollLimit ))
		{
			sceneEvent.hasPassedBeat ( currentBeatD );
		}
	}

	private void PreProcessEvents ( ) {
		foreach (TimelyEvent t in BeatmapEvents.events)
		{
			if (t is IntroduceTrack ev)
			{
				ev.SetTrackMetronome( metronome );
			}
		}
	}


}
