using snaresJ.script.beatmaps.Events;

public partial class Game {
    public void ProcessBeatmapEvents ( ) {
        var currentBeatD = metronome.GetCurrentTotalBeats ();
        foreach (TimelyEvent sceneEvent in BeatmapEvents.sceneEvents.GetNext ( BeatmapEvents.sceneEventPollLimit ))
        {
            if (!sceneEvent.hasPassedBeat ( currentBeatD )) continue;
            if (sceneEvent is StartDisplayingTrack SDTEvent) sliders.AddTrackToScene ( SDTEvent.track );
            if (sceneEvent is IntroduceTrack ITEvent)
                sliders.sliders.Find ( track => track.track == ITEvent.GetTrackObject () ).Active = true;
        }

        foreach (TimelyEvent rhythmObject in BeatmapEvents.rhythmObjects.GetNext (
                     BeatmapEvents.rhythmObjectPollLimit
                 ))
        {
            if (rhythmObject is Snare snare && snare.hasPassedBefore ( currentBeatD, metronome.Numerator ))
            {
                // TODO: introduce the snare to the track
            }

        }
    }

    public void ProcessUserInput ( ) {

    }
}