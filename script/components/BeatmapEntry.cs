using Godot;
using System;
using snaresJ.script.beatmaps;
using snaresJ.script.filesystem;
using snaresJ.script.state;
using snaresJ.script.utility;

public partial class BeatmapEntry : HBoxContainer
{

	public Beatmap beatmap = new();

	private RichTextLabel beatmapTitle;
	private RichTextLabel beatmapDesc;

	public void Enlarge ( int px, float time ) {
		StandardTween.TweenQ (
			this,
			"custom_minimum_size",
			new Vector2 ( 0, 200 + px ), time
		);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		MouseFilter = MouseFilterEnum.Stop;
		// get nodes
		beatmapTitle = GetNode<RichTextLabel> ("beatmapInfo/beatmapTitle");
		beatmapDesc = GetNode<RichTextLabel> ("beatmapInfo/beatmapDesc");
		foreach (Node child in GetChildren ())
		{
			if (child is Control control && control != this)
			{
				control.MouseFilter = MouseFilterEnum.Ignore;
			}

			if (child is RichTextLabel label)
			{
				label.MouseFilter = MouseFilterEnum.Ignore;
			}
		}

		GuiInput += ev =>
		{
			if ((ev is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: false } ||
				 ev is InputEventScreenTouch { Pressed: false } ) && beatmap.Validity)
			{
				State.instance.hasSelectedBeatmap = true;
				State.instance.selectedBeatmap = beatmap;
				GD.Print ( "Selected a beatmap " + beatmap.songName + " - " + beatmap.songArtist );
			}
		};

		MouseEntered += ( ) =>
		{
			Enlarge ( 50, 0.5f );
		};
		MouseExited += ( ) =>
		{
			Enlarge ( 0, 0.5f );
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (!beatmap.Validity)
		{
			beatmapTitle.Text = "Invalid beatmap!";
			beatmapDesc.Text = "Did someone mess with the files?";
		}

		// update text
		beatmapTitle.Text = "" + beatmap.songName + " - " +  beatmap.songArtist;
		beatmapDesc.Text = "" + beatmap.BPM + " - " + beatmap.Difficulty + " - " + beatmap.mapper;
	}
}
