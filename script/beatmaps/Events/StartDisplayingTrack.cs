namespace snaresJ.script.beatmaps.Events;

public class StartDisplayingTrack : TimelyEvent {

    public static TimelyEvent AddIntroTrackEv ( IntroduceTrack introduceTrackEvent ) {
        var theBeat = introduceTrackEvent.beat - introduceTrackEvent.GetTrackObject ().introductionBeats;
        if (theBeat <= 0)
        {
            return new Nothing ();
        }

        return new StartDisplayingTrack ( )
        {
            beat = theBeat
        };
    }

}