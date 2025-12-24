using System;
using Godot;
using Godot.Collections;
using snaresJ.script.state;
using Array = Godot.Collections.Array;

namespace snaresJ.script.loading;

public partial class Loading : ColorRect
{
	private ResourceLoader.ThreadLoadStatus? status;
	private Array progress = [];


	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		status = null;
		progress = [];
		if (!State.IsSetup ()) return;
		if (!State.instance.currentlyLoading)
		{
			// Not currently loading? Maybe the scenes were switched improperly!
			if (State.instance.loadingFrom == "") throw new Exception ( "There is no current loading path to loading from." );
			GetTree().ChangeSceneToFile (State.instance.loadingFrom);
			return;
		}

		ResourceLoader.LoadThreadedRequest ( State.instance.pathToLoad );

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {

		status = ResourceLoader.LoadThreadedGetStatus ( State.instance.pathToLoad, progress );
		var progressBar = GetNode<ProgressBar>("progress");

		if (progress.Count > 0)
		{
			var v = progress[0].AsDouble ();
			progressBar.Value = v;
			Color = new Color ( (float) v, (float) v, (float) v);
		}

		if (status is ResourceLoader.ThreadLoadStatus.Failed or ResourceLoader.ThreadLoadStatus.InvalidResource)
		{
			State.instance.currentlyLoading = false;
			State.instance.lastLoadSuccessful = false;
			GetTree().ChangeSceneToFile (State.instance.loadingFrom);
		}

		if (status != ResourceLoader.ThreadLoadStatus.Loaded) return;
		var res = ResourceLoader.LoadThreadedGet ( State.instance.pathToLoad );
		if (res is not PackedScene scene) return;
		State.instance.currentlyLoading = false;
		State.instance.lastLoadSuccessful = false;
		GetTree ().ChangeSceneToPacked ( scene );

	}
}
