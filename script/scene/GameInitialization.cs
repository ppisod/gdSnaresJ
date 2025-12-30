using Godot;
using snaresJ.script.scene;
using snaresJ.script.state;
using snaresJ.script.utility.Rhythm;

public partial class Game {
	private void initializeChildren ( ) {
		back = GetNode<SlipButton>("Quit");
		back.buttonLabelText = "quit";
		back.clicked += ( ) =>
		{
			State.instance.LoadLargeScene ( Scenes.TITLE, Scenes.BEATMAPS );
		};
		sliders = GetNode<SliderContainer> ("body/game/sliderContainer");
	}

	private void initializeMetronome ( ) {
		metronome = new Metronome(bm.BPM, bm.TimeSignatureNumerator, bm.TimeSignatureDenominator);

		countInBeats = bm.CountInBars * bm.TimeSignatureNumerator;
	}

	private void initializeAudio ( ) {
		songAudio = GD.Load <AudioStream> ( bm.songPath );
		audioPlayer = new AudioStreamPlayer ();
		AddChild ( audioPlayer );
		audioPlayer.Stream = songAudio;
	}
}
