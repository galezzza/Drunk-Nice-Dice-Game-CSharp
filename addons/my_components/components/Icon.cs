using Godot;
using System;
using System.ComponentModel.Design;

[Tool]

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

	private static Color[] colorsList = {
			Globals.colorsDictionaryGroup["main"]["white"],
			Globals.colorsDictionaryGroup["main"]["black"],
			Globals.colorsDictionaryGroup["main"]["primary"],
			Globals.colorsDictionaryGroup["main"]["secondary"],
			Globals.colorsDictionaryGroup["main"]["neutral"],
			Globals.colorsDictionaryGroup["primary"]["900"],
			Globals.colorsDictionaryGroup["neutral"]["900"],
			Globals.colorsDictionaryGroup["main"]["error"],
		};
	
	private IconColors _colorIcon = IconColors.White;
	[Export] public IconColors ColorIcon
	{
		get => _colorIcon;
		set
		{
			_colorIcon = value;
			SetColor(value);
		}
	}
	

	public Icon()
	{
		this.ColorIcon = IconColors.White;
	}

	public override void _Ready()
	{
		int index = (int) ColorIcon;
		UpdateIconColor(index);
	}


	private void UpdateIconColor(int colorIndex)
	{	
		int index = (int) colorIndex;

		SelfModulate = colorsList[index];
	}

	public void SetColor(IconColors iconColor)
	{
		int index = (int) iconColor;
		UpdateIconColor(index);
	}

	public Color GetColorByEnum(IconColors iconColor){
		int index = (int) iconColor;
		return colorsList[index];
	}
	
}
