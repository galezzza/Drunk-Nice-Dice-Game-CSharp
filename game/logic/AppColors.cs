using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public partial class AppColors : Node
{
	// Spliting colors was used to generate DerivedColor based on MainColors later.
	// I want to add feathure that allows user to change themes.
	private MainColors mainColors = new MainColors(new Color("849BEC"), new Color("D9EDFC"), new Color("BD33A4"));
	private DerivedColors derivedColors = new DerivedColors(new Color("849BEC"), new Color("D9EDFC"), new Color("BD33A4"));

	public Color[] GetAllColorsArray()
	{   
		Color[] mainColorsArray = mainColors.GetMainColorsArray();
		Color[] derivedColorsArray = derivedColors.GetDerivedColorsArray();

		Color[] colors = new Color[mainColorsArray.Length + derivedColorsArray.Length];
		
		int startCopyIndex = 0;
		mainColorsArray.CopyTo(colors, startCopyIndex);

		startCopyIndex = mainColorsArray.Length;
		derivedColorsArray.CopyTo(colors, startCopyIndex);
		
		return colors;
	}
	public Color[][] GetAllColorsArrayGroup(){
		Color[][] colors = new Color[5][];
		colors[0] = mainColors.GetMainColorsArray();
		colors[1] = derivedColors.GetAllPrimaryColorsArray();
		colors[2] = derivedColors.GetAllSecondaryColorsArray();
		colors[3] = derivedColors.GetAllNeutralColorsArray();
		colors[4] = derivedColors.GetAllOpacityColorsArray();
		return colors;
	}
	public Dictionary<string, Dictionary<string, Color>> GetAllColorsDictionaryGroup(){
		Dictionary<string, Dictionary<string, Color>> colors = new Dictionary<string, Dictionary<string, Color>>
		{
			{"main", mainColors.GetMainColorsDictionary()},
			{"primary", derivedColors.GetPrimaryColorsDictionary()},
			{"secondary", derivedColors.GetSecondaryColorsDictionary()},
			{"neutral", derivedColors.GetNeutralColorsDictionary()},
			{"opacity", derivedColors.GetOpacityColorsDictionary()},
		};
		return colors;
	}

	public Dictionary<string, Color> GetAllColorsDictionary()
	{	
		Dictionary<string, Color> colors = new Dictionary<string, Color>();
		Dictionary<string, Dictionary<string, Color>> colorsGroup = GetAllColorsDictionaryGroup();

		for (int indexColorType = 0; indexColorType < colorsGroup.Count; indexColorType++)
		{
			string colorsTypeName = colorsGroup.ElementAt(indexColorType).Key;
			Dictionary<string, Color> colorsNames = colorsGroup.ElementAt(indexColorType).Value;

			for (int indexColorName = 0; indexColorName < colorsNames.Count; indexColorName++)
			{
				string colorName = colorsNames.ElementAt(indexColorName).Key;
				Color color = colorsNames.ElementAt(indexColorName).Value;

				colorName = ColorNaming(colorName, colorsTypeName, indexColorType);

				colors.Add(colorName, color);
			}
		}
		return colors;
	}

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
}


internal class MainColors
{
	private Color primary ;
	private Color secondary;
	private Color neutral;
	private Color error;
	private Color white;
	private Color black;


	public MainColors(Color primary, Color secondary, Color error)
	{
		this.primary = primary;
		this.secondary = secondary;
		this.error = error;
		this.neutral = CreateNeutralColorByPrimary(this.primary);
		this.white = new Color("white");
		this.black = new Color("black");
	}

	private Color CreateNeutralColorByPrimary(Color primary)
	{
		return new Color("EDEFF8");
	}

	public Color[] GetMainColorsArray()
	{   
		Color[] colors = new Color[] {this.primary, this.secondary, this.neutral, this.error};
		return colors;
	}
	public Dictionary<string, Color> GetMainColorsDictionary()
	{   
		Dictionary<string, Color> colors = new Dictionary<string, Color>
		{
			{"primary", this.primary}, 
			{"secondary", this.secondary}, 
			{"neutral", this.neutral},
			{"error", this.error},
			{"white", this.white}, 
			{"black", this.black}, 
		};
		return colors;
	}

}


internal class DerivedColors
{
	private Color primary50;
	private Color primary100;
	private Color primary200;
	private Color primary300;
	private Color primary400;
	private Color primary500;
	private Color primary600;
	private Color primary700;
	private Color primary800;
	private Color primary900;

	private Color secondary50;
	private Color secondary100;
	private Color secondary200;
	private Color secondary300;
	private Color secondary400;
	private Color secondary500;
	private Color secondary600;
	private Color secondary700;
	private Color secondary800;
	private Color secondary900;

	private Color neutral50;
	private Color neutral100;
	private Color neutral200;
	private Color neutral300;
	private Color neutral400;
	private Color neutral500;
	private Color neutral600;
	private Color neutral700;
	private Color neutral800;
	private Color neutral900;

	private Color opacityPrimary8;
	private Color opacityPrimary12;
	private Color opacitySecondary8;
	private Color opacitySecondary12;
	private Color opacityBlack8;
	private Color opacityBlack12;
	private Color opacityBlack16;

	public DerivedColors(Color primary, Color secondary, Color neutral)
	{
		this.primary50 = new Color("F3F5FD");
		this.primary100 = new Color("D9E0F9");
		this.primary200 = new Color("C6D1F6");
		this.primary300 = new Color("ADBCF2");
		this.primary400 = new Color("9DAFF0");
		this.primary500 = new Color("849BEC");
		this.primary600 = new Color("788DD7");
		this.primary700 = new Color("5E6EA8");
		this.primary800 = new Color("495582");
		this.primary900 = new Color("374163");

		this.secondary50 = new Color("FBFDFF");
		this.secondary100 = new Color("F3F9FE");
		this.secondary200 = new Color("EEF7FE");
		this.secondary300 = new Color("E6F3FD");
		this.secondary400 = new Color("E1F1FD");
		this.secondary500 = new Color("D9EDFC");
		this.secondary600 = new Color("C5D8E5");
		this.secondary700 = new Color("9AA8B3");
		this.secondary800 = new Color("77828B");
		this.secondary900 = new Color("5B646A");

		this.neutral50 = new Color("FDFDFE");
		this.neutral100 = new Color("F9FAFD");
		this.neutral200 = new Color("F7F8FC");
		this.neutral300 = new Color("F3F4FA");
		this.neutral400 = new Color("F1F2F9");
		this.neutral500 = new Color("EDEFF8");
		this.neutral600 = new Color("D8D9E2");
		this.neutral700 = new Color("A8AAB0");
		this.neutral800 = new Color("828388");
		this.neutral900 = new Color("646468");

		this.opacityPrimary8 = primary;
		this.opacityPrimary8.A = 0.08f;

		this.opacityPrimary12 = primary;
		this.opacityPrimary12.A = 0.12f;

		this.opacitySecondary8 = secondary;
		this.opacitySecondary8.A = 0.08f;

		this.opacitySecondary12 = secondary;
		this.opacitySecondary12.A = 0.12f;

		this.opacityBlack8 = new Color(0,0,0,0.08f);
		this.opacityBlack12 = new Color(0,0,0,0.12f);
		this.opacityBlack16 = new Color(0,0,0,0.16f);
	}

	public Color[] GetDerivedColorsArray()
	{
		Color[] colors = new Color[] {primary50, primary100, primary200, primary300, primary400, primary500, primary600,
		primary700, primary800, primary900, secondary50, secondary100, secondary200, secondary300, secondary400,
		secondary500, secondary600, secondary700, secondary800, secondary900, neutral50, neutral100, neutral200,
		neutral300, neutral400, neutral500, neutral600, neutral700, neutral800, neutral900, opacityPrimary8, opacityPrimary12,
		opacitySecondary8, opacitySecondary12, opacityBlack8, opacityBlack12, opacityBlack16};
		return colors;
	}
	
	public Color[] GetAllPrimaryColorsArray(){
		Color[] colors = new Color[] {primary50, primary100, primary200, primary300, primary400, primary500, primary600,
		primary700, primary800, primary900};
		return colors;
	}
	public Dictionary<string, Color> GetPrimaryColorsDictionary()
	{   
		Dictionary<string, Color> colors = new Dictionary<string, Color>
		{
			{"50", this.primary50}, 
			{"100", this.primary100}, 
			{"200", this.primary200}, 
			{"300", this.primary300}, 
			{"400", this.primary400}, 
			{"500", this.primary500}, 
			{"600", this.primary600}, 
			{"700", this.primary700}, 
			{"800", this.primary800}, 
			{"900", this.primary900}, 
		};
		return colors;
	}

	public Color[] GetAllSecondaryColorsArray(){
		Color[] colors = new Color[] {secondary50, secondary100, secondary200, secondary300, secondary400,
		secondary500, secondary600, secondary700, secondary800, secondary900};
		return colors;
	}
	public Dictionary<string, Color> GetSecondaryColorsDictionary()
	{   
		Dictionary<string, Color> colors = new Dictionary<string, Color>
		{
			{"50", this.secondary50}, 
			{"100", this.secondary100}, 
			{"200", this.secondary200}, 
			{"300", this.secondary300}, 
			{"400", this.secondary400}, 
			{"500", this.secondary500}, 
			{"600", this.secondary600}, 
			{"700", this.secondary700}, 
			{"800", this.secondary800}, 
			{"900", this.secondary900}, 
		};
		return colors;
	}

	public Color[] GetAllNeutralColorsArray(){
		Color[] colors = new Color[] {neutral50, neutral100, neutral200,
		neutral300, neutral400, neutral500, neutral600, neutral700, neutral800, neutral900};
		return colors;
	}
	public Dictionary<string, Color> GetNeutralColorsDictionary()
	{   
		Dictionary<string, Color> colors = new Dictionary<string, Color>
		{
			{"50", this.neutral50}, 
			{"100", this.neutral100}, 
			{"200", this.neutral200}, 
			{"300", this.neutral300}, 
			{"400", this.neutral400}, 
			{"500", this.neutral500}, 
			{"600", this.neutral600}, 
			{"700", this.neutral700}, 
			{"800", this.neutral800}, 
			{"900", this.neutral900}, 
		};
		return colors;
	}

	public Color[] GetAllOpacityColorsArray()
	{
		Color[] colors = new Color[] {opacityPrimary8, opacityPrimary12,
		opacitySecondary8, opacitySecondary12, opacityBlack8, opacityBlack12, opacityBlack16};
		return colors;
	}
	public Dictionary<string, Color> GetOpacityColorsDictionary()
	{   
		Dictionary<string, Color> colors = new Dictionary<string, Color>
		{
			{"primary8", this.opacityPrimary8}, 
			{"primary12", this.opacityPrimary12}, 
			{"secondary8", this.opacitySecondary8}, 
			{"secondary12", this.opacitySecondary12}, 
			{"black8", this.opacityBlack8}, 
			{"black12", this.opacityBlack12}, 
			{"black16", this.opacityBlack16}, 
		};
		return colors;
	}
}

