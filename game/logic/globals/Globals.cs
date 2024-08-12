using Godot;
using System;
using System.Collections.Generic;


public partial class Globals : Node
{
	// public static Color[] colorsArray = new AppColors().GetAllColorsArray();
	// public static Color[][] colorsArrayGroup = new AppColors().GetAllColorsArrayGroup();
	public static Dictionary<string, Dictionary<string, Color>> colorsDictionary = new AppColors().GetAllColorsDictionaryGroup();
}
