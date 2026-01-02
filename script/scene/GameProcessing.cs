using Godot;
using snaresJ.script.beatmaps.Events;

public partial class Game {
	public void ProcessBeatmapEvents ( ) {
		var currentBeatD = metronome.GetCurrentTotalBeats ();
		int sceneEventIteration = 0;
		foreach (TimelyEvent sceneEvent in BeatmapEvents.sceneEvents.GetNext ( BeatmapEvents.sceneEventPollLimit ))
		{
			if (sceneEvent is null)
			{
				GD.PrintErr ( "scene event is null???!?!??" );
				continue;
			}

			if (sceneEvent.processed) continue;
			if (!sceneEvent.hasPassedBeat ( currentBeatD )) continue;
			if (sceneEvent is StartDisplayingTrack SDTEvent)
			{
				sliders.AddTrackToScene ( SDTEvent.track, true );
				sceneEvent.processed = true;
				sceneEventIteration++;
				continue;
			}
			if (sceneEvent is IntroduceTrack ITEvent)
			{

				var s = sliders.sliders.Find ( track => track.track == ITEvent.GetTrackObject () );
				if (s == null)
				{
					GD.PrintErr ( "Could not find slider for track " + ITEvent.GetTrackObject ().id );
					continue;
				}

				s.Active = true;
				sceneEvent.processed = true;
				sceneEventIteration++;

			}
		}

		BeatmapEvents.sceneEvents.IncrementIndex ( sceneEventIteration );

		foreach (TimelyEvent rhythmObject in BeatmapEvents.rhythmObjects.GetNext (
					 BeatmapEvents.rhythmObjectPollLimit
				 ))
		{
			if (rhythmObject is Snare snare && snare.hasPassedBefore ( currentBeatD, metronome.Numerator * SnareFadeTime ))
			{
				// TODO: introduce the snare to the track
				// Set Procesed -> true
			}

		}
	}

	public void ProcessUserInput ( ) {

	}

	public void ProcessDebug ( ) {
		if (sliders == null)
		{
			GD.PrintErr ( "sliders is null???" );
			return;
		}

		foreach (SliderTrack slider in sliders.sliders)
		{
			if (slider == null)
			{
				GD.PrintErr ( "slider is null???" );
			}
			else
			{
				if (slider.track == null)
				{
					GD.PrintErr ( "slider track is null???" );
				}
			}
		}
	}
}
