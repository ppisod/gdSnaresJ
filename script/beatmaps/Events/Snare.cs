using System;
using Godot;
using Godot.Collections;
using snaresJ.script.utility;

namespace snaresJ.script.beatmaps.Events;

public class Snare : TimelyEvent {
    public int trackId = 0;

    public Snare ( Dictionary e ) {
        string eventType = L.V <string> ( e, "event", Variant.Type.String );
        if (eventType != "snare") throw new ArgumentException ( "unsupported event type: " + eventType + " passed into snare" );
        trackId = L.V <int> ( e, "trackId", Variant.Type.Int );
    }
}