using System;
using System.Collections.Generic;
using Godot;
using snaresJ.script.beatmaps.Enum;
using snaresJ.script.utility;

namespace snaresJ.script.beatmaps.Events;

public class EventCollection ( double initialBeatsPerMinute ) {

    public Tracker <TimelyEvent> events = new ();
    public double initialBeatsPerMinute = initialBeatsPerMinute; // TODO: set all BPM objects to be float/double

    public Tracker <TimelyEvent> rhythmObjects = null!;
    public Tracker <TimelyEvent> sceneEvents = null!;

    // anything exceeding these will not be processed; only the next 50 will be processed per tick
    public int rhythmObjectPollLimit = 50;
    public int sceneEventPollLimit = 15;

    public void Add ( TimelyEvent evt ) {
        // find where to add the event.
        // TODO: This function can actually be reversed for optimality,
        // because normally, events are going to be read top-down, and the time of the events should
        // be ascending, so therefore, if we reverse it, the for loop
        // below will go through less iterations.
        // This can also be substituted with a binary search.

        // Decide insertion index
        int indx = 0;
        if (events.Count > 0) {
            double tsNew = evt.timestampInMs(initialBeatsPerMinute);
            for (; indx < events.Count; indx++) {
                double tsList = events[indx].timestampInMs(initialBeatsPerMinute);
                if (Math.Abs(tsList - tsNew) < 0.0001d || tsNew < tsList) {
                    break; // insert here
                }
            }
        }

        events.Insert(indx, evt);

        // Always perform custom behavior for the event that was just added
        CustomBehaviourAdd(evt);
    }

    public void SortEventsIntoCategories ( ) {
        rhythmObjects = [];
        sceneEvents = [];
        foreach ( var evt in events )
        {
            if (evt is IntroduceTrack) sceneEvents.Add ( evt );
            if (evt is StartDisplayingTrack) sceneEvents.Add ( evt );

            if (evt is Snare) rhythmObjects.Add ( evt );
        }
    }

    private void CustomBehaviourAdd ( TimelyEvent evt ) {
        GD.Print ( "Custom Behaviour Add called" );
        if (evt is IntroduceTrack e)
        {
            GD.Print ( "Custom Behaviour Add for IntroduceTrack event added" );
            var s = StartDisplayingTrack.AddIntroTrackEv ( e );
            Add ( s );
        }
    }
}