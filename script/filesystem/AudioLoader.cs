#nullable enable
using System;
using System.IO;
using Godot;

namespace snaresJ.script.filesystem;

/// <summary>
/// Utility for loading audio streams from res:// (imported resources) and user:// (runtime files).
/// </summary>
public static class AudioLoader
{
    public static AudioStream? Load(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            GD.PushError("AudioLoader.Load: path is null or empty");
            return null;
        }

        // Normalize slashes
        path = path.Replace('\\', '/');

        if (path.StartsWith("res://", StringComparison.OrdinalIgnoreCase))
        {
            var stream = ResourceLoader.Load<AudioStream>(path);
            if (stream == null)
            {
                GD.PushError($"AudioLoader.Load: Failed to load AudioStream from {path}");
            }
            return stream;
        }

        var localized = ProjectSettings.LocalizePath(path);
        if (!string.IsNullOrEmpty(localized))
        {
            path = localized;
        }

        // For user:// (and other readable paths), load bytes and construct
        if (path.StartsWith("user://", StringComparison.OrdinalIgnoreCase) || Godot.FileAccess.FileExists(path))
        {
            if (!Godot.FileAccess.FileExists(path))
            {
                GD.PushError($"AudioLoader.Load: File not found: {path}");
                return null;
            }

            byte[] bytes = Godot.FileAccess.GetFileAsBytes(path);
            if (bytes == null || bytes.Length == 0)
            {
                GD.PushError($"AudioLoader.Load: Could not read bytes: {path}");
                return null;
            }

            var ext = Path.GetExtension(path).ToLowerInvariant();
            return FromBytes(bytes, ext);
        }

        GD.PushError($"AudioLoader.Load: Unsupported path: {path}");
        return null;
    }

    /// <summary>
    /// Attempts to load an AudioStream. Returns true on success.
    /// </summary>
    public static bool TryLoad(string path, out AudioStream? stream)
    {
        stream = Load(path);
        return stream != null;
    }

    private static AudioStream? FromBytes(byte[] bytes, string ext)
    {
        switch (ext)
        {
            case ".mp3":
                var mp3 = new AudioStreamMP3 { Data = bytes };
                return mp3;
            case ".wav":
                var wav = new AudioStreamWav { Data = bytes };
                return wav;
            default:
                GD.PushError($"AudioLoader: Unsupported audio extension '{ext}'. Supported: .mp3, .wav");
                return null;
        }
    }
}
