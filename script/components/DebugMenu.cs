using Godot;
using System;

public partial class DebugMenu : Control {

	public bool wronged = false;
	public bool initialized = false;
	public Game game = null!;
	public RichTextLabel label = null!;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {

		var gaem = GetTree ().GetCurrentScene ();

		if (!(gaem is Game))
		{
			GD.PrintErr ( "DebugMenu should only be in Game scene!" );
			wronged = true;
		}

		if (!wronged)
		{
			var gmem = gaem as Game;
			game = gmem;
		}

		label = GetNode<RichTextLabel> ("highbox/container/debugtext");
		if (label == null) GD.PrintErr ( "DebugMenu should have RichTextLabel named 'debugtext'" );

		initialized = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		if (wronged || !initialized) return;
		label.Text = game.state.ToString ();
	}
}
