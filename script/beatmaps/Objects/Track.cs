using System.Collections.Generic;
using snaresJ.script.beatmaps.Enum;
using snaresJ.script.utility.Rhythm;

namespace snaresJ.script.beatmaps.Objects;

public class Track {
    public List <Ticker> tickers;
    public TriggerRequirement req;
    public int id;
    public int beatsPerLength;
    public Metronome usingMetronome;
    public double whichBeatStartedOn;
}