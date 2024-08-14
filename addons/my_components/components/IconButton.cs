using Godot;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class IconButton : Button
{

	private Control stateLayerContainer;
	private Icon icon;

	public override void _Ready()
	{	
		stateLayerContainer = GetNode<PanelContainer>("%State-layer container");
		this.Theme = InitializeTheme(this.Theme, this.Name);

		StyleBox stylebox = this.Theme.GetStylebox("enabled", this.Name);
		stateLayerContainer.AddThemeStyleboxOverride("panel", stylebox);
		
		stateLayerContainer.MouseFilter = MouseFilterEnum.Ignore;

		this.SelfModulate = new Color(0,0,0,0);
		this.FocusMode = FocusModeEnum.None;

		if (this.Disabled)
		{
			SetDisabled();
		}
		else
		{
			this.MouseEntered += MouseEnteredHandler;
			this.MouseExited += MouseExitedHandler;
			this.ButtonDown += ButtonDownHandler;
			this.ButtonUp += ButtonUpHandler;
			this.FocusEntered += FocusEnteredHandler;
			this.FocusExited += FocusExitedHandler;
		}
		
	}

	private Theme InitializeTheme(Theme inputTheme, string inputIhemeType)
	{
		Theme theme = inputTheme;
		string themeType = inputIhemeType;

		InitializeThemeColors(theme, themeType);
		Dictionary<string, Color> colorsForTheme = new Dictionary<string, Color>
		{
			{"enabled", new Color(0, 0, 0, 0)},
			{"hovered", theme.GetColor("opacityPrimary8", themeType)},
			{"focused", theme.GetColor("opacityPrimary12", themeType)},
			{"pressed", theme.GetColor("opacityPrimary12", themeType)},
			{"disabled", new Color(0, 0, 0, 0)},
		};
		InitializeStyleBoxes(theme, themeType, colorsForTheme);

		return	theme;
	}

	private void InitializeThemeColors(Theme inputTheme, string inputIhemeType)
	{	
		Theme theme = inputTheme;
		string themeType = inputIhemeType;
		
		string FirstLetterToUpper(string str)
		{
			str = str.Substring(0, 1).ToUpper() + str.Substring(1);
			return str;
		}
		
		string ColorNaming(string colorName, string colorsTypeName, int indexColorType)
		{
			colorName = FirstLetterToUpper(colorName);

			int indexOfColorsTypeThatWontBeRenamed = 0;
			if (indexColorType != indexOfColorsTypeThatWontBeRenamed)
			{
				colorName = colorsTypeName + colorName;
			}
			return colorName;
		}

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

				theme.SetColor(colorName, themeType, color);
			}
		}

		inputTheme = theme;
	}

	private void InitializeStyleBoxes(Theme inputTheme, string inputIhemeType
	, Dictionary<string, Color> inputColorsForStyleboxes)
	{		
		Theme theme = inputTheme;
		string themeType = inputIhemeType;
		Dictionary<string, Color> colors = inputColorsForStyleboxes;

		string[] styleboxNames = theme.GetStyleboxList(themeType);
		int numberOfStyleboxes = styleboxNames.Length;

		Debug.Assert(colors.Count == numberOfStyleboxes, 
			"Number of colors is not equal to number of styleboxes");
		
		StyleBoxFlat AllCornerRadiuses(StyleBoxFlat styleBox, int value)
		{
			styleBox.CornerRadiusBottomLeft = value;
			styleBox.CornerRadiusBottomRight = value;
			styleBox.CornerRadiusTopLeft = value;
			styleBox.CornerRadiusTopRight = value;
			return styleBox;
		}

		StyleBoxFlat InitializeSingleStylebox(Color color)
		{	
			StyleBoxFlat styleBox = new StyleBoxFlat();

			styleBox = new StyleBoxFlat();
			styleBox.BgColor = color;
			styleBox = AllCornerRadiuses(styleBox, 100);

			return styleBox;
		}

		StyleBoxFlat styleBox;
		for (int i = 0; i < numberOfStyleboxes; i++)
		{	
			string styleBoxName = styleboxNames[i];

			styleBox = InitializeSingleStylebox(colors[styleBoxName]);
			theme.SetStylebox(styleboxNames[i], themeType, styleBox);
		}

		inputTheme = theme;
	}

	private void SetDisabled()
	{
		stateLayerContainer = GetNode<PanelContainer>("%State-layer container");

		StyleBox stylebox = this.Theme.GetStylebox("disabled", this.Name);
		stateLayerContainer.AddThemeStyleboxOverride("panel", stylebox);

		icon = GetNode<Icon>("%Icon");
		icon.SetColor(global::Icon.IconColors.Neutral900);
		
		Color iconModulate = icon.Modulate;
		iconModulate.A = 0.38f;
		icon.Modulate = iconModulate;
	}

	private void ButtonUpHandler()
	{
		stateLayerContainer = GetNode<PanelContainer>("%State-layer container");

		StyleBox stylebox = this.Theme.GetStylebox("enabled", this.Name);
		if (this.IsHovered())
		{
			stylebox = this.Theme.GetStylebox("hovered", this.Name);
		}

		stateLayerContainer.AddThemeStyleboxOverride("panel", stylebox);
	}

	private void ButtonDownHandler()
	{
		stateLayerContainer = GetNode<PanelContainer>("%State-layer container");

		StyleBox stylebox = this.Theme.GetStylebox("pressed", this.Name);
		stateLayerContainer.AddThemeStyleboxOverride("panel", stylebox);
	}

	private void FocusExitedHandler()
	{
		stateLayerContainer = GetNode<PanelContainer>("%State-layer container");

		StyleBox stylebox = this.Theme.GetStylebox("enabled", this.Name);
		stateLayerContainer.AddThemeStyleboxOverride("panel", stylebox);
	}

	private void FocusEnteredHandler()
	{
		stateLayerContainer = GetNode<PanelContainer>("%State-layer container");

		StyleBox stylebox = this.Theme.GetStylebox("focused", this.Name);
		stateLayerContainer.AddThemeStyleboxOverride("panel", stylebox);
	}

	private void MouseExitedHandler()
	{
		stateLayerContainer = GetNode<PanelContainer>("%State-layer container");

		StyleBox stylebox = this.Theme.GetStylebox("enabled", this.Name);
		stateLayerContainer.AddThemeStyleboxOverride("panel", stylebox);
	}

	private void MouseEnteredHandler()
	{
		stateLayerContainer = GetNode<PanelContainer>("%State-layer container");

		StyleBox stylebox = this.Theme.GetStylebox("hovered", this.Name);
		stateLayerContainer.AddThemeStyleboxOverride("panel", stylebox);
	}
}
