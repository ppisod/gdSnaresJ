using System;
using System.Collections.Generic;
using snaresJ.script.beatmaps.Enum;

namespace snaresJ.script.beatmaps.Events;

public class EventCollection {
    public List <TimelyEvent> events = new ();
    public List <double> eventsTimestamps = new ();
    public double initialBeatsPerMinute; // TODO: set all BPM objects to be float/double

    public EventCollection ( double initialBeatsPerMinute ) {
        this.initialBeatsPerMinute = initialBeatsPerMinute;
    }

    public void Add ( TimelyEvent evt ) {
        // find where to add the event.
        // TODO: This function can actually be reversed for optimality,
        // because normally, events are going to be read top-down, and the time of the events should
        // be ascending, so therefore, if we reverse it, the for loop
        // below will go through less iterations.
        // This can also be substituted with a binary search.
        if (events.Count == 0)
        {
            events.Add(evt);
            eventsTimestamps.Add ( evt.timestampInMs ( initialBeatsPerMinute ) );
            return;
        }
        int indx = 0;
        for ( ; indx < events.Count; indx++ )
        {
            var timestampListItem = events[indx].timestampInMs ( initialBeatsPerMinute );
            var timestampNewItem = evt.timestampInMs ( initialBeatsPerMinute );
            if ( Math.Abs ( timestampListItem - timestampNewItem ) < 0.0001d || timestampNewItem < timestampListItem )
            {
                events.Insert ( indx, evt );
                eventsTimestamps.Insert ( indx, timestampNewItem );
                return;
            }
        }
        events.Insert ( indx, evt );
        eventsTimestamps.Insert ( indx, evt.timestampInMs ( initialBeatsPerMinute ) );
    }
}