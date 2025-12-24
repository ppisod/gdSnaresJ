using Godot;

namespace snaresJ.script.filesystem;

public class FileHelper ( string basePath = "" ) {
	// Always open at user:// root to avoid null when infoPath doesn't exist yet
	private readonly DirAccess dirAccess = DirAccess.Open("user://");
	private string _basePath = basePath;

	public string GetMountedFilepath() {
		// Ensure a proper user:// absolute path, with single separators
		if (string.IsNullOrEmpty(_basePath)) return "user://";
		return _basePath.StartsWith("user://") ? _basePath : Combine("user://", _basePath);
	}

	private static string Combine(params string[] parts)
	{
		if (parts == null || parts.Length == 0) return string.Empty;
		string result = parts[0] ?? string.Empty;
		for (int i = 1; i < parts.Length; i++)
		{
			var p = parts[i] ?? string.Empty;
			if (string.IsNullOrEmpty(p)) continue;
			bool needSlash = !result.EndsWith("/") && !p.StartsWith("/");
			bool doubleSlash = result.EndsWith("/") && p.StartsWith("/");
			if (needSlash)
				result += "/" + p;
			else if (doubleSlash)
				result += p.TrimStart('/');
			else
				result += p;
		}
		return result;
	}

	/// <summary>
	/// creates a directory at filepath dirName at user://
	/// </summary>
	/// <param name="dirName"></param>
	/// <returns>if a new directory was made</returns>
	public bool CreateDirectory ( string dirName ) {

		var dp = Combine(GetMountedFilepath(), dirName);
		if (DirAccess.DirExistsAbsolute(dp))
		{
			GD.Print ( "Creating \"" + dirName + "\" from \"" + _basePath + "\" skipped because already exists" );

			return false;
		}

		Error error = DirAccess.MakeDirRecursiveAbsolute(dp);
		if (error == Error.Ok)
		{
			GD.Print ( "Created \"" + dirName + "\" from \"" + _basePath + "\" finished!" );
			return true;
		}

		GD.PrintErr($"Failed to create directory '{dp}'. Error: {error}");
		return false;

	}

	/// <summary>
	/// Creates a file at filepath.
	/// </summary>
	/// <param name="fileName">filename. defaults to NoFileName</param>
	/// <param name="overwrite">overwrite option?</param>
	/// <param name="text">default write text</param>
	/// <returns></returns>
	public bool CreateFile ( string fileName = "NoFileName", bool overwrite = false, string text = " " ) {

		var fp = Combine(GetMountedFilepath(), fileName);

		if (!overwrite && FileAccess.FileExists ( fp ))
		{
			GD.Print ( "No overwrite parameter and file already exists. Skipping writing \"" + fp + "\"!");
			return false;
		}

		// Ensure parent directory exists
		var lastSlash = fp.LastIndexOf('/')
						;
		if (lastSlash > "user://".Length)
		{
			var parent = fp.Substring(0, lastSlash);
			if (!DirAccess.DirExistsAbsolute(parent))
			{
				var derr = DirAccess.MakeDirRecursiveAbsolute(parent);
				if (derr != Error.Ok)
				{
					GD.PrintErr($"Failed to create parent directory '{parent}'. Error: {derr}");
					return false;
				}
			}
		}

		using FileAccess fileAccess = FileAccess.Open ( fp, FileAccess.ModeFlags.WriteRead );
		if (fileAccess == null)
		{
			GD.PrintErr ( "FileAccess is null! Making file failed!!! error= " + FileAccess.GetOpenError() );
			return false;
		}
		fileAccess.StoreString ( text );
		return true;

	}

	public Error cd ( string dirName ) {

		Error r = dirAccess.ChangeDir ( dirName );
		var abs = dirAccess.GetCurrentDir();
		// keep _basePath relative to avoid double user:// later
		_basePath = abs.StartsWith("user://") ? abs.Substring("user://".Length) : abs;
		return r;
	}

	public DirAccess getDirAccess() {
		return dirAccess;
	}

}
