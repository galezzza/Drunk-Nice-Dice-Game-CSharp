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
		Color[] colorsList = {
			Globals.colorsDictionary["main"]["white"],
			Globals.colorsDictionary["main"]["black"],
			Globals.colorsDictionary["main"]["primary"],
			Globals.colorsDictionary["main"]["secondary"],
			Globals.colorsDictionary["main"]["neutral"],
			Globals.colorsDictionary["primary"]["900"],
			Globals.colorsDictionary["neutral"]["900"],
			Globals.colorsDictionary["main"]["error"],

		};

		int index = (int) colorIndex;

		SelfModulate = colorsList[index];
	}
}
	// public static Color white = new Color("FFFFFF");
	// public static Color black = new Color("000000");
	// public static Color primary = new Color("849BEC");
	// public static Color secondary = new Color("D9EDFC");
	// public static Color neutral = new Color("EDEFF8");
	// public static Color primary900 = new Color("374163");
	// public static Color neutral900 = new Color("646468");
	// public static Color error = new Color("BD33A4");
