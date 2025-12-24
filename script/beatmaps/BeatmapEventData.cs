using Godot;
using Godot.Collections;
using snaresJ.script.beatmaps.Events;
using snaresJ.script.utility;

namespace snaresJ.script.beatmaps;

public partial class Beatmap {
    public bool HasLoadedEvents = false;
    public EventCollection BeatmapEvents;

    public int TimeSignatureNumerator = 4;
    public int TimeSignatureDenominator = 4;

    public float startMs;
    public float startBeat;
    public float endBeat;

    public int CountInBars;

    public void LoadEvents ( ) {
        FileAccess fileAccess = FileAccess.Open ( beatmapPath, FileAccess.ModeFlags.Read );
        if (fileAccess == null)
        {
            GD.PrintErr ( "Can't open beatmap file! might be invalid or missing? Passed in beatmap path: \"" + beatmapPath + "\"" );
            return;
        }

        var beatmapObject = Json.ParseString(fileAccess.GetAsText());
        if (beatmapObject.VariantType != Variant.Type.Dictionary)
        {
            GD.PrintErr ( "beatmap object not a dictionary?" );
            return;
        }

        Dictionary beatmap = (Dictionary) beatmapObject;

        startMs = L.V <float> ( beatmap, "startMs", Variant.Type.Float );
        startBeat = L.V <float> ( beatmap, "startBeats", Variant.Type.Float );
        endBeat = L.V <float> ( beatmap, "endBeats", Variant.Type.Float );
        TimeSignatureNumerator = L.V <int> (beatmap, "tsig_num",  Variant.Type.Int);
        TimeSignatureDenominator = L.V <int> ( beatmap, "tsig_denom", Variant.Type.Int);
        CountInBars = L.V <int> ( beatmap, "countInBars", Variant.Type.Int);
        BPM = L.V <double> ( beatmap, "bpm", Variant.Type.Float );

        Array gameEvents = L.V <Array> ( beatmap, "gameEvents", Variant.Type.Array );

        BeatmapEvents = new EventCollection ( BPM );

        foreach (Variant variantGameEvent in gameEvents)
        {
            if (variantGameEvent.VariantType != Variant.Type.Dictionary)
            {
                GD.PrintErr ( "game event not a dictionary?" );
            }
            Dictionary gameEvent = (Dictionary) variantGameEvent;

            TimelyEvent timelyEvent = TimelyEvent.parseEventFromDictionary ( gameEvent );

            BeatmapEvents.Add (timelyEvent);
        }

        HasLoadedEvents = true;
    }
}