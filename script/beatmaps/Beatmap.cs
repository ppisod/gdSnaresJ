
using System;
using Godot;
using Godot.Collections;
using snaresJ.script.beatmaps.Events;
using snaresJ.script.filesystem;

namespace snaresJ.script.beatmaps;

public partial class Beatmap {

    public readonly string songName = "songName";
    public readonly string songArtist = "songArtist";
    public readonly string mapper = "mapper";
    public double BPM = 0;
    public readonly string Difficulty = "difficulty";
    public readonly string beatmapPath = FilePaths.FILEPATH_BEATMAPS;
    public readonly string songPath = FilePaths.FILEPATH_BEATMAPS;
    public readonly string infoPath = "";
    public string Identifier = "11111-00000-00001";
    public readonly bool Validity = false;


    public Beatmap (
        string name,
        string artist,
        string mapper,
        float bpm,
        string beatmapPath,
        string songPath,
        string infoPath,
        string identifier,
        bool validity
    ) {
        songName = name;
        songArtist = artist;
        this.mapper = mapper;
        BPM = bpm;
        this.beatmapPath += beatmapPath;
        this.songPath += songPath;
        this.infoPath += infoPath;
        Identifier = identifier;
        Validity = validity;
    }

    public Beatmap ( ) {

    }

    public Beatmap ( string infoPath ) {
        FileAccess fileAccess = FileAccess.Open ( infoPath, FileAccess.ModeFlags.Read );
        if (fileAccess == null)
        {
            GD.PrintErr ( "Can't open info file! might be invalid or missing? Passed in info path: \"" + infoPath + "\"" );
            return;
        }

        Variant infoObject = Json.ParseString(fileAccess.GetAsText());
        
        // this is the beatmap object (demo)
        // {
        //     "songname":"Run To The Sun",
        //     "songpath":"music/song.mp3",
        //     "artistname":"N.E.R.D",
        //     "mappername":"caltr4",
        //     "bpm":97,
        //     "identifiera":0,
        //     "identifierb":0,
        //     "identifierc":2,
        //     "beatmapPath":"beatmap"
        // }

        this.infoPath = infoPath;
        var path = infoPath.Remove ( infoPath.IndexOf ( "info", StringComparison.Ordinal ) );

        if (infoObject.VariantType != Variant.Type.Dictionary)
        {
            GD.PrintErr("Beatmap info JSON is not an object. beatmapPath: " + infoPath);
            return;
        }

        var dict = (Dictionary)infoObject;

        // Strings
        if (dict.TryGetValue("songname", out var vSongName) && vSongName.VariantType == Variant.Type.String)
            songName = (string) vSongName;
        if (dict.TryGetValue("artistname", out var vArtist) && vArtist.VariantType == Variant.Type.String)
            songArtist = (string) vArtist;
        if (dict.TryGetValue("mappername", out var vMapper) && vMapper.VariantType == Variant.Type.String)
            mapper = (string) vMapper;
        if (dict.TryGetValue("beatmapPath", out var vPath) && vPath.VariantType == Variant.Type.String)
            beatmapPath = path + (string) vPath;
        if (dict.TryGetValue("songpath", out var vSongPath) && vSongPath.VariantType == Variant.Type.String)
            songPath = path + (string) vSongPath;
        if (dict.TryGetValue("difficulty", out var vDifficulty) && vDifficulty.VariantType == Variant.Type.String)
            Difficulty = (string) vDifficulty;

        // BPM number: JSON may deliver int or float
        if (dict.TryGetValue("bpm", out var vBpm))
        {
            if (vBpm.VariantType == Variant.Type.Int)
                BPM = (int)(long)vBpm;
            else if (vBpm.VariantType == Variant.Type.Float)
                BPM = (double)vBpm;
        }

        int ida = 0, idb = 0, idc = 1;
        if (dict.TryGetValue("identifiera", out var vIda))
        {
            if (vIda.VariantType == Variant.Type.Int) ida = (int)(long)vIda; else if (vIda.VariantType == Variant.Type.Float) ida = (int)(double)vIda;
        }
        if (dict.TryGetValue("identifierb", out var vIdb))
        {
            if (vIdb.VariantType == Variant.Type.Int) idb = (int)(long)vIdb; else if (vIdb.VariantType == Variant.Type.Float) idb = (int)(double)vIdb;
        }
        if (dict.TryGetValue("identifierc", out var vIdc))
        {
            if (vIdc.VariantType == Variant.Type.Int) idc = (int)(long)vIdc; else if (vIdc.VariantType == Variant.Type.Float) idc = (int)(double)vIdc;
        }
        Identifier = ida + "-" + idb + "-" + idc;


        Validity = !string.IsNullOrEmpty(songName) &&
                   !string.IsNullOrEmpty(mapper) &&
                   !string.IsNullOrEmpty(beatmapPath) &&
                   !string.IsNullOrEmpty(songPath) &&
                   !string.IsNullOrEmpty(this.infoPath) &&
                   !string.IsNullOrEmpty(songArtist);
    }

    public static Beatmap makeInvalidBeatmap ( ) {
        return new Beatmap ();
    }
}