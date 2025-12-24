using Godot;
using System;
using System.Collections.Generic;
using snaresJ.script.beatmaps;
using snaresJ.script.filesystem;
using snaresJ.script.scene;
using snaresJ.script.state;

public partial class BeatmapSelection : Control {

	private SlipButton back;
	private SlipButton play;
	private VBoxContainer entries;

	private Timer detectBeatmapTimer;
	private bool timerTriggered;
	public readonly List<Beatmap> beatmapsDetected = [];
	public readonly Dictionary <string, bool> pathsDetected = [];

	private PackedScene beatmapEntryScene;

	public void DetectBeatmaps ( ) {
		timerTriggered = true;
		DirAccess dirAccess = DirAccess.Open( "user://" + FilePaths.FILEPATH_BEATMAPS );
		string[] directories = dirAccess.GetDirectories ();
		foreach (string directory in directories)
		{
			// go to the beatmap folder
			dirAccess.ChangeDir ( directory );
			if (!dirAccess.FileExists ( "info" ))
			{
				continue;
			}
			Beatmap beatmap = new Beatmap ( dirAccess.GetCurrentDir (  ) + "/info" );
			if ( !beatmapsDetected.Contains ( beatmap ) )
			{
				beatmapsDetected.Add ( beatmap );
			}

			if (!pathsDetected.ContainsKey ( dirAccess.GetCurrentDir (  ) + "/info" ))
			{
				pathsDetected[dirAccess.GetCurrentDir ( ) + "/info"] = false;
			}
		}

	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		beatmapsDetected.Clear();
		foreach (Beatmap beatmap in State.instance.loadedBeatmaps)
		{
			beatmapsDetected.Add ( beatmap );
		}

		back = GetNode<SlipButton> ("back");
		back.buttonLabelText = "back";

		play = GetNode<SlipButton> ("play");
		play.buttonLabelText = "play";
		play.SetVisible ( false );

		entries = GetNode<VBoxContainer> ("VBody/Selector/scrollable/beatmaps");
		beatmapEntryScene = ResourceLoader.Load<PackedScene> ("res://components/beatmapEntry.tscn");

		detectBeatmapTimer = GetNode<Timer> ("detectBeatmaps");

		back.GuiInput += @event =>
		{
			if ( @event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: false } ||
				 @event is InputEventScreenTouch { Pressed: false } )
			{
				State.instance.LoadLargeScene ( Scenes.BEATMAPS, Scenes.TITLE );
			}
		};

		play.GuiInput += @event =>
		{
			if ( @event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: false } ||
				 @event is InputEventScreenTouch { Pressed: false } )
			{
				State.instance.LoadLargeScene ( Scenes.BEATMAPS, Scenes.GAME );
			}
		};

		detectBeatmapTimer.Start();
		detectBeatmapTimer.Timeout += DetectBeatmaps;

		DetectBeatmaps();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		play.SetVisible ( State.instance.hasSelectedBeatmap );
		if (timerTriggered)
		{
			detectBeatmapTimer.Start();
			State.instance.loadedBeatmaps = new List <Beatmap> (beatmapsDetected);
			timerTriggered = false;
		}

		foreach (Beatmap beatmap in beatmapsDetected)
		{
			if (pathsDetected.TryGetValue ( beatmap.infoPath, out var hasRendered ) && hasRendered)
			{
				continue;
			}
			pathsDetected[beatmap.infoPath] = true;
			BeatmapEntry entry = beatmapEntryScene.Instantiate<BeatmapEntry>();
			entry.beatmap = beatmap;
			entries.AddChild ( entry );
			GD.Print ( "Added a beatmap to the screen!" );
		}
	}
}
