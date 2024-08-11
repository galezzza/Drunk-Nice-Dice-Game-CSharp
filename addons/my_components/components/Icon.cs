using Godot;
using System;
using System.ComponentModel.Design;

public partial class Icon : TextureRect
{
	enum Colors
	{
		White,
		Black,
		Primary,
		Secondary, 
		Neutral,
		Primary900,
		Neutral900,
		Error
	}
	
	[Export]
	private Colors colors = Colors.White;

	public override void _Ready()
	{
		int index = (int) colors;
		UpdateIconColor(index);
	}


	private void UpdateIconColor(int colorIndex)
	{	
		Color[] colorsList = {Globals.white, Globals.black, Globals.primary, Globals.secondary,
								 Globals.neutral, Globals.primary900, Globals.neutral900, Globals.error};

		int index = (int) colorIndex;

		SelfModulate = colorsList[index];
	}
}
