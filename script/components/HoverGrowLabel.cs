using Godot;
using System;
using snaresJ.script.utility;

public partial class HoverGrowLabel : RichTextLabel {

	public int normalSize = 120;
	public int growSize = 150;
	public float duration = 0.5f;

	public override void _Ready() {
		MouseFilter = MouseFilterEnum.Stop;
		AddThemeFontSizeOverride("normal_font_size", normalSize);
		MouseEntered += ( ) =>
		{
			StandardTween.TweenQ (
				this,
				"theme_override_font_sizes/normal_font_size",
				growSize,
				duration
			);
		};
		MouseExited += ( ) =>
		{
			StandardTween.TweenQ (
				this,
				"theme_override_font_sizes/normal_font_size",
				normalSize,
				duration
			);
		};
	}
}
