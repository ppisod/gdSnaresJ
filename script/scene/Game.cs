using Godot;
using System;
using snaresJ.script.beatmaps;
using snaresJ.script.beatmaps.Events;
using snaresJ.script.scene;
using snaresJ.script.state;
using snaresJ.script.utility.Rhythm;

public partial class Game : Control {
	private SlipButton back;

	private double timeInScene;

	private Beatmap BeatmapPlaying;
	private EventCollection BeatmapEvents;

	private Metronome metronome;

	private bool playing = false;
	private AudioStream songAudio;
	private AudioStreamPlayer audioPlayer;

	public int countInBeats;
	public int trueBeats = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		back = GetNode<SlipButton>("Quit");
		back.buttonLabelText = "quit";
		back.clicked += ( ) =>
		{
			State.instance.LoadLargeScene ( Scenes.TITLE, Scenes.BEATMAPS );
		};
		GD.Print ( State.instance.selectedBeatmap.beatmapPath );
		BeatmapPlaying = State.instance.selectedBeatmap;

		// load events
		BeatmapPlaying.LoadEvents();

		// this is when the BPM is correctly set
		metronome = new Metronome(BeatmapPlaying.BPM, BeatmapPlaying.TimeSignatureNumerator, BeatmapPlaying.TimeSignatureDenominator);

		countInBeats = BeatmapPlaying.CountInBars * BeatmapPlaying.TimeSignatureNumerator;

		BeatmapEvents = BeatmapPlaying.BeatmapEvents;

		// load the music
		songAudio = GD.Load <AudioStream> ( BeatmapPlaying.songPath );
		audioPlayer = new AudioStreamPlayer ();
		AddChild ( audioPlayer );
		audioPlayer.Stream = songAudio;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		timeInScene += delta;
		var beats = metronome.TotalBeats;
		if (!playing)
		{
			if (beats == countInBeats) // TODO: this is a buggy check
			{
				playing = true;

			}

			return;
		}

	}


}
