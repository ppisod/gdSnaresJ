using System.Collections.Generic;
using snaresJ.script.beatmaps.Enum;

namespace snaresJ.script.beatmaps.Objects;

public class Track {
    public List <Ticker> tickers;
    public TriggerRequirement req;
    public int id;
    public int beatsPerLength;
}