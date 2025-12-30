namespace snaresJ.script.beatmaps.Events;

public class StartDisplayingTrack : TimelyEvent {
    public StartDisplayingTrack ( IntroduceTrack introduceTrackEvent ) {
        beat = introduceTrackEvent.beat - introduceTrackEvent.GetTrackObject ().introductionBeats;
    }
}