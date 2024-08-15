using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class themeScript : Theme
{
	private string FirstLetterToUpper(string str)
	{
		str = str.Substring(0, 1).ToUpper() + str.Substring(1);
		return str;
	}

	private string ColorNaming(string colorName, string colorsTypeName, int indexColorType)
		{
			colorName = FirstLetterToUpper(colorName);

			int indexOfColorsTypeThatWontBeRenamed = 0;
			if (indexColorType != indexOfColorsTypeThatWontBeRenamed)
			{
				colorName = colorsTypeName + colorName;
			}
			return colorName;
		}

	public void InitializeThemeColors(string inputIhemeType)
	{	
		string themeType = inputIhemeType;

		Dictionary<string, Dictionary<string, Color>> colors = Globals.colorsDictionary;

		for (int indexColorType = 0; indexColorType < colors.Count; indexColorType++)
		{
			string colorsTypeName = colors.ElementAt(indexColorType).Key;
			Dictionary<string, Color> colorsNames = colors.ElementAt(indexColorType).Value;

			for (int indexColorName = 0; indexColorName < colorsNames.Count; indexColorName++)
			{
				string colorName = colorsNames.ElementAt(indexColorName).Key;
				Color color = colorsNames.ElementAt(indexColorName).Value;

				colorName = ColorNaming(colorName, colorsTypeName, indexColorType);

				this.SetColor(colorName, themeType, color);
			}
		}
	}
}
