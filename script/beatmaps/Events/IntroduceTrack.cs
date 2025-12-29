using System;
using Godot;
using Godot.Collections;
using snaresJ.script.beatmaps.Enum;
using snaresJ.script.beatmaps.Objects;
using snaresJ.script.utility;
using Array = Godot.Collections.Array;

namespace snaresJ.script.beatmaps.Events;

public class IntroduceTrack : TimelyEvent {

    private readonly Track trackToBeIntroduced;

    public Track GetTrackObject ( ) {
        return trackToBeIntroduced;
    }

    /// <summary>
    /// To be removed
    /// </summary>
    public IntroduceTrack ( ) {
        trackToBeIntroduced = new Track
        {
            req = TriggerRequirement.KEY,
            id = 0
        };
        isValid = true;
        trackToBeIntroduced.tickers.Add ( new Ticker ( 0, Tween.EaseType.In, Tween.TransitionType.Linear ) );
        trackToBeIntroduced.tickers.Add ( new Ticker ( 0.25, Tween.EaseType.In, Tween.TransitionType.Linear ) );
        trackToBeIntroduced.tickers.Add ( new Ticker ( 5, Tween.EaseType.In, Tween.TransitionType.Linear ) );
        trackToBeIntroduced.tickers.Add ( new Ticker ( 0.75, Tween.EaseType.In, Tween.TransitionType.Linear ) );
    }

    public IntroduceTrack ( Dictionary e ) {
        trackToBeIntroduced = new Track ();

        string eventName = L.V <string> ( e, "event", Variant.Type.String );
        if (eventName != "introduceTrack") throw new ArgumentException("Unknown event name passed into IntroduceTrack: " + eventName);

        int trackId = (int) L.N ( e, "trackId");
        trackToBeIntroduced.id = trackId;

        string requires = L.V <string> ( e, "requires", Variant.Type.String );
        switch (requires)
        {
            case "key":
                trackToBeIntroduced.req = TriggerRequirement.KEY;
                break;
            case "mouse":
                trackToBeIntroduced.req = TriggerRequirement.MOUSE;
                break;
            default:
                trackToBeIntroduced.req = TriggerRequirement.KEY;
                break;
        }

        int beatsPerLength = (int) L.N ( e, "beatsPerLength");
        trackToBeIntroduced.beatsPerLength = beatsPerLength;

        Array tickers = L.V<Array> ( e, "tickers", Variant.Type.Array );
        foreach ( var ticker in tickers )
        {
            if (ticker.VariantType != Variant.Type.Dictionary) throw new ArgumentException("Unknown ticker variant type. Must be a dictionary: " + ticker.VariantType);
            var tickerDict = (Dictionary) ticker;

            Ticker t = new Ticker (tickerDict);
            trackToBeIntroduced.tickers.Add ( t );
        }
        isValid = true;
    }


}