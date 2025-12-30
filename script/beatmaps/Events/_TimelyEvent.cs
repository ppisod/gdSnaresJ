using System;
using Godot;
using Godot.Collections;
using snaresJ.script.beatmaps.Enum;
using snaresJ.script.utility;

namespace snaresJ.script.beatmaps.Events;

public class TimelyEvent {
    public bool isValid = false;
    public double time;
    public double beat;
    public TimingUsing timingUsing;

    // Events to add:
    /*
     * stopMetronome
     * delayTime
     * setMetronomeBpm
     * stopTrack
     * editTrack
     */

    public static TimelyEvent parseEventFromDictionary ( Dictionary e ) {
        string type = L.V <string> ( e, "event", Variant.Type.String );

        // Parse with null/default tracking
        bool hasBeat = e.ContainsKey("beat");
        bool hasTime = e.ContainsKey("time");

        float b = hasBeat ? L.V<float>(e, "beat", Variant.Type.Float) : 0f;
        float t = hasTime ? L.V<float>(e, "time", Variant.Type.Float) : 0f;

        TimelyEvent tim;
        switch (type)
        {
            case "introduceTrack":
                tim = new IntroduceTrack ( e );
                break;
            case "snare":
                tim = new Snare ( e );
                break;
            default:
                throw new NotImplementedException ( "not implemented timely event of type: " + type );
        }


        if (hasTime && !hasBeat)
        {
            tim.timingUsing = TimingUsing.TIME;
            tim.time = t;
        }
        else if (hasBeat && !hasTime)
        {
            tim.timingUsing = TimingUsing.BEAT;
            tim.beat = b;
        }
        else if (hasBeat)
        {

            tim.timingUsing = TimingUsing.BEAT;
            tim.beat = b;
            tim.time = t;
        }
        else
        {
            tim.timingUsing = TimingUsing.BEAT;
            tim.beat = 0;
        }

        return tim;
    }

    public double timestampInMs ( double bpm ) {
        var tu = timingUsing;
        if (tu == TimingUsing.TIME)
        {
            return time;
        }

        double beatsPerMillisecond =  bpm / 60d / 1000d;
        return beatsPerMillisecond * beat;
    }

}