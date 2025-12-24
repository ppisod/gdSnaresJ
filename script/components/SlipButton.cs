using Godot;
using System;
using snaresJ.script.utility;

public partial class SlipButton : Control {

	public string buttonLabelText = "button";

	private float offsetDefault = 0f;

	private float initialLeft = 0f;
	private float initialTop = 0f;
	private float initialRight = 0f;
	private float initialBottom = 0f;

	public float upByHowMuch = 0.1f;
	public float upByHowMuchWhenClicked = 0.15f;
	public float outlineNewHowMuchPixels = 5f;

	public float time = 0.3f;

	public event Action clicked;

	private RichTextLabel label;
	private TextureRect outline;

	public void ClickOn ( )  {
		StandardTween.TweenQ (
			this,
			"anchor_top",
			initialTop - upByHowMuchWhenClicked,
			time
		);
		StandardTween.TweenQ (
			this,
			"anchor_bottom",
			initialBottom - upByHowMuchWhenClicked,
			time
		);
	}

	public void HoverOn ( ) {
		// tween this node
		StandardTween.TweenQ (
			this,
			"anchor_top",
			initialTop - upByHowMuch,
			time
		);
		StandardTween.TweenQ (
			this,
			"anchor_bottom",
			initialBottom - upByHowMuch,
			time
		);

		// tween outline
		StandardTween.TweenQ (
			outline,
			"offset_left",
			-offsetDefault - outlineNewHowMuchPixels,
			time
		);
		StandardTween.TweenQ (
			outline,
			"offset_top",
			-offsetDefault - outlineNewHowMuchPixels,
			time
		);
		StandardTween.TweenQ (
			outline,
			"offset_right",
			offsetDefault + outlineNewHowMuchPixels,
			time
		);
		StandardTween.TweenQ (
			outline,
			"offset_bottom",
			offsetDefault + outlineNewHowMuchPixels,
			time
		);
	}

	public void HoverOff ( ) {
		StandardTween.TweenQ (
			this,
			"anchor_top",
			initialTop,
			time
		);
		StandardTween.TweenQ (
			this,
			"anchor_bottom",
			initialBottom,
			time
		);

		StandardTween.TweenQ (
			outline,
			"offset_left",
			-offsetDefault,
			time
		);
		StandardTween.TweenQ (
			outline,
			"offset_top",
			-offsetDefault,
			time
		);
		StandardTween.TweenQ (
			outline,
			"offset_right",
			offsetDefault,
			time
		);
		StandardTween.TweenQ (
			outline,
			"offset_bottom",
			offsetDefault,
			time
		);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {

		initialLeft = (float) Get ( "anchor_left" );
		initialRight = (float) Get ( "anchor_right" );
		initialTop = (float) Get ( "anchor_top" );
		initialBottom = (float) Get ( "anchor_bottom" );

		label = GetNode<RichTextLabel> ("RichTextLabel");
		label.Text = buttonLabelText;

		outline = GetNode<TextureRect> ("outline");
		offsetDefault = (float) outline.Get ( "offset_right" );

		MouseFilter = MouseFilterEnum.Stop;
		MouseEntered += HoverOn;
		MouseExited += HoverOff;

		GuiInput += @event =>
		{
			if ( @event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: false } ||
				 @event is InputEventScreenTouch { Pressed: false } )
			{
				clicked?.Invoke ();
			}

			if ( @event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true })
			{
				ClickOn ();
			}
		};

		Set ( "anchor_top", initialTop + 0.5f );
		Set ( "anchor_bottom", initialBottom + 0.5f );
		SetVisible ( true );
		StandardTween.TweenQ (
			this,
			"anchor_top",
			initialTop,
			0.7f
		);
		StandardTween.TweenQ (
			this,
			"anchor_bottom",
			initialBottom,
			0.7f
		);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		label.Text = buttonLabelText;
	}
}
