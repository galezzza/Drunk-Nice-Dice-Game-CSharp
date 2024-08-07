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
	public override void _Process(double delta)
	{
	}

	private void UpdateIconColor(int colorIndex)
	{	
		Color white = new Color("FFFFFF");
		Color black = new Color("000000");
		Color primary = new Color("849BEC");
		Color secondary = new Color("D9EDFC");
		Color neutral = new Color("EDEFF8");
		Color primary900 = new Color("374163");
		Color neutral900 = new Color("646468");
		Color error = new Color("BD33A4");

		Color[] colorsList = {white, black, primary, secondary, neutral, primary900, neutral900, error};

		int index = (int) colorIndex;
		var image = Texture.GetImage();
		SelfModulate = colorsList[index];
	}
}
