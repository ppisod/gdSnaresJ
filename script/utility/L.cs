using System;
using Godot;
using Godot.Collections;
using Godot.NativeInterop;
using Microsoft.VisualBasic;

namespace snaresJ.script.utility;

public static class L {

	public static T V <[MustBeVariant] T> ( Dictionary e, string name, Variant.Type vType ) {
		e.TryGetValue ( name, out var k );
		// @ToBeRemoved
		GD.Print($"Type of '{name}': {k.VariantType}, Value: {k}");
		if (k.VariantType != vType)
		{
			throw new ArgumentException ( name + " argument type didn't match." );
		}

		T v = k.As <T> ();
		return v;
	}

	public static float N ( Dictionary e, string name ) {
		e.TryGetValue ( name, out var k );
		// @ToBeRemoved
		GD.Print($"Type of '{name}': {k.VariantType}, Value: {k}");
		if (k.VariantType != Variant.Type.Float && k.VariantType != Variant.Type.Int)
		{
			throw new ArgumentException ( name + " argument type (supposed to be a number) didn't match." );
		}
		return (float) k;
	}

	public static T VD <[MustBeVariant] T> ( Dictionary e, string name, Variant.Type vType, T defaultV) {
		e.TryGetValue ( name, out var k );
		// @ToBeRemoved
		GD.Print($"Type of '{name}': {k.VariantType}, Value: {k}");
		if (k.VariantType == Variant.Type.Nil) return defaultV;
		if (k.VariantType != vType)
		{
			throw new ArgumentException ( name + " argument type didn't match." );
		}

		T v = k.As <T> ();
		return v;
	}

}
