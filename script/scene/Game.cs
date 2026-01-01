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
	private bool syncedAfterDelay = false;

	private bool playing = false;
	private AudioStream songAudio;
	private AudioStreamPlayer audioPlayer;

	public GameState state = GameState.LOADING;

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
		state = GameState.COUNTDOWN;
		// begin tej count-in immediately so beats can advance
		metronome.Start();

	}

	public override void _Process(double delta)
	{
		if (!initialized)
		{
			return;
		}

		// update time
		timeInScene += delta;
		// advance metronome timing
		metronome.Update(TimeSpan.FromSeconds(delta));
		var beats = metronome.TotalBeats;
		GD.Print ( beats );


		if (!playing)
		{
			if (beats >= countInBeats) // start playing after count-in
			{
				playing = true;
				PlaySequence ();
			}
			return;
		}

		// While we're waiting to sync with audio start, count down startDelay
		if (startDelay > 0)
		{
			startDelay -= delta;
			return;
		}

		// Once the delay finishes, sync metronome with audio exactly once
		if (!syncedAfterDelay)
		{
			startDelay = 0;
			metronome.Stop();
			metronome.Reset();
			metronome.Start();
			syncedAfterDelay = true;
		}
		ProcessDebug ();
		// Process Events
		ProcessBeatmapEvents ();
		ProcessUserInput ();
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
		metronome.Start ();
		audioPlayer.Play ();
		// initial startDelay
		startDelay = bm.startMs;
		state = GameState.PLAYING;
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
