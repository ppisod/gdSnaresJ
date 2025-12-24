using Godot;
using snaresJ.script.filesystem;
using snaresJ.script.state;

namespace snaresJ.script.scene;

public partial class Default : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Initialize FS
		FileHelper helper = new FileHelper();
		helper.CreateDirectory ( FilePaths.FILEPATH_BEATMAPS_NAME );
		Error r = helper.cd ( FilePaths.FILEPATH_BEATMAPS_NAME );
		if (r != Error.Ok)
		{
			GD.PrintErr ( r );
		}
		helper.CreateFile ( FilePaths.FILEPATH_BEATMAPS_README_NAME, false, "This is the folder where all of your beatmaps are stored. Drag unzipped folders into this directory to load beatmaps!" );
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (State.IsSetup ())
		{
			State.instance.LoadLargeScene ( Scenes.DEFAULT_SCENE, Scenes.TITLE );
		}
	}
}
