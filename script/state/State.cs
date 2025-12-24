#nullable enable
using System;
using System.Collections.Generic;
using Godot;
using snaresJ.script.beatmaps;
using snaresJ.script.scene;

namespace snaresJ.script.state;

/// <summary>
/// A singleton Node to handle the game state.
/// </summary>
public partial class State : Node {

	public static State instance = null!;

	public const string DEFAULT_SCENE = "";
	public bool currentlyLoading = false;
	public bool lastLoadSuccessful = true;
	public string loadingFrom = "";
	public string pathToLoad = "";
	public PackedScene? loadedScene;

	public bool hasSelectedBeatmap = false;
	public Beatmap selectedBeatmap = null!;
	public List <Beatmap> loadedBeatmaps = [];

	public State ( ) {
		if ( instance != null )
		{
			throw new NotSupportedException ( "only ONE instance allowed for State!" );
		}

		instance = this;
	}
	public void LoadLargeScene ( string from, string scenePath ) {
		GetTree().ChangeSceneToFile ( Scenes.LOADING );
		currentlyLoading = true;
		loadingFrom = from;
		pathToLoad = scenePath;
	}

	public void DirectlyLoad ( string scenePath ) {
		GetTree().ChangeSceneToFile ( scenePath );
	}

	public static bool IsSetup ( ) {
		return instance != null;
	}

	public void quit ( ) {
		GetTree().Quit ();
	}

}