using Godot;

namespace snaresJ.script.utility;

public static class StandardTween {
    public static void TweenQ (
        Node node,
        NodePath property,
        Variant value,
        float duration
    ) {
        Tween tween = node.CreateTween ();
        PropertyTweener t = tween.TweenProperty ( node, property, value, duration );
        t.SetEase ( Tween.EaseType.Out );
        t.SetTrans ( Tween.TransitionType.Quad );
        tween.Play ();
    }
}