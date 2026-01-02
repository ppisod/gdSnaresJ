using Godot;
using snaresJ.script.beatmaps.Objects;

namespace snaresJ.script.beatmaps.Events;

public class StartDisplayingTrack : TimelyEvent {

    public Track track;

    public static TimelyEvent AddIntroTrackEv ( IntroduceTrack introduceTrackEvent ) {
        var theBeat = introduceTrackEvent.beat - introduceTrackEvent.GetTrackObject ().introductionBeats;
        if (theBeat <= 0)
        {
            GD.PrintErr ( "Track introduction beat is before the first beat." );
            return new StartDisplayingTrack ( )
            {
                beat = 0,
                track = introduceTrackEvent.GetTrackObject ()
            };
        }

        return new StartDisplayingTrack ( )
        {
            beat = theBeat,
            track = introduceTrackEvent.GetTrackObject ()
        };
    }

}