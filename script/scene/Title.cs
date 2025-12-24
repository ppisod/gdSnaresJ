using Godot;
using System;
using snaresJ.script.scene;
using snaresJ.script.state;
using snaresJ.script.utility;

public partial class Title : Control {
	private HoverGrowLabel play;
	private HoverGrowLabel quit;
	private HoverGrowLabel config;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {

		play = GetNode <HoverGrowLabel> ( "buttons/play" );
		quit = GetNode <HoverGrowLabel> ( "buttons/quit" );
		config = GetNode <HoverGrowLabel> ( "buttons/config" );

		play.GuiInput += @event =>
		{
			if ( @event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: false } ||
				 @event is InputEventScreenTouch { Pressed: false } )
			{
				State.instance.LoadLargeScene ( Scenes.TITLE, Scenes.BEATMAPS );
			}
		};
		quit.GuiInput += @event =>
		{
			if ( @event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: false } ||
				 @event is InputEventScreenTouch { Pressed: false } )
			{
				State.instance.quit ();
			}
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process (double delta)
	{

	}
}
