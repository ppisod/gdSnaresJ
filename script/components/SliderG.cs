using Godot;
using System;

public partial class SliderG : Control {

	public Slider slider;
	public bool ready = false;

	public override void _Ready() {
		slider = GetNode <Slider> ( "slider" );
		ready = true;
	}


	public override void _Process(double delta)
	{

	}
}
