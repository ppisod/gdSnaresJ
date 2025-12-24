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

	public int PreBeats;
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

		metronome.Start (); // give BeatmapPlaying.CountInBars bars before starting.

		BeatmapEvents = BeatmapPlaying.BeatmapEvents;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		timeInScene += delta;
	}
}
