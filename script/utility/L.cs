using System;
using Godot;
using Godot.Collections;
using Godot.NativeInterop;

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