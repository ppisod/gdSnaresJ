using System;
using Godot;
using Godot.Collections;

namespace snaresJ.script.beatmaps.Objects;

public class Ticker {
    public readonly double at = 0.0;
    public readonly Tween.EaseType directionType = Tween.EaseType.InOut;
    public readonly Tween.TransitionType transType = Tween.TransitionType.Linear;

    public Ticker ( double At_OutOfLength, Tween.EaseType direction, Tween.TransitionType trans ) {
        at = At_OutOfLength >= 1 || At_OutOfLength < 0 ? 0 : At_OutOfLength;
        directionType = direction;
        transType = trans;
    }

    public Ticker ( ) {

    }

    public Ticker ( Dictionary dictFromBeatmap ) {

        dictFromBeatmap.TryGetValue ( "at", out var ValueAt);
        dictFromBeatmap.TryGetValue ( "easingTransitionToThisTicker", out var ValueDirection);
        dictFromBeatmap.TryGetValue ( "easingDirectionToThisTicker", out var ValueTrans);

        at = ValueAt.VariantType == Variant.Type.Int ||
             ValueAt.VariantType == Variant.Type.Float ?
            (double) ValueAt :
            0.0d;

        string dir = ValueDirection.VariantType == Variant.Type.String ? (string) ValueDirection : "";
        string trans = ValueTrans.VariantType == Variant.Type.String ? (string) ValueTrans : "";

        if (dir == "inout") directionType = Tween.EaseType.InOut;
        else if (dir == "outin") directionType = Tween.EaseType.OutIn;
        else if (dir == "out") directionType = Tween.EaseType.Out;
        else if (dir == "in") directionType = Tween.EaseType.Out;
        else throw new ArgumentException ( "dir in Ticker object is not valid! must be: inout, outin, out, in" );

        if (trans == "quad") transType = Tween.TransitionType.Quad;
        else if (trans == "linear") transType = Tween.TransitionType.Linear;
        else if (trans == "quint") transType = Tween.TransitionType.Quint;
        else if (trans == "spring") transType = Tween.TransitionType.Spring;
        else if (trans == "bounce") transType = Tween.TransitionType.Bounce;
        else throw new ArgumentException ( "trans in Ticker object is not valid! must be: quad, linear, quint, spring, bounce" );

    }
}