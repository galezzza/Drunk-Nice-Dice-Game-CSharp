using Godot;
using System;
using System.ComponentModel.Design;

public partial class Icon : TextureRect
{
	public enum IconColors
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
	private IconColors color = IconColors.White;

	public Icon()
	{
		this.color = IconColors.White;
	}

	public override void _Ready()
	{
		int index = (int) color;
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

	public void SetColor(IconColors iconColor)
	{
		int index = (int) iconColor;
		UpdateIconColor(index);
	}
	
}
