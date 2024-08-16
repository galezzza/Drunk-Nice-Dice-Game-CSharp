using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

[Tool]

public partial class IconButton : Button
{
	private PanelContainer stateLayerContainer = new PanelContainer();
	private Icon icon = new Icon();


	private static int iconButtonSize = 48;
	private static int stateLayerContainerSize = 40;

	private Texture2D _texture;

	[Export]
	public Texture2D IconTexture
	{
		get => _texture;
		set
		{
			_texture = value;
			OnTextureSet(value);
		}
	}

	private void OnTextureSet(Texture2D texture)
	{
		icon.Texture = texture;
	}

	public override void _Ready()
	{	
		if (Engine.IsEditorHint())
		{
			// Code to execute when in editor.
			EditorPreview();
		}

		if (!Engine.IsEditorHint())
		{
			// Code to execute when in game.
			RunReadyAtGame();
		}
	}

	private void EditorPreview()
	{
		InitializeScene();

		StyleBoxFlat styleBox12 = new StyleBoxFlat();
		styleBox12.BgColor = new Color("849BEC", 0.08f);
		styleBox12 = AllCornerRadiuses(styleBox12, 100);
		stateLayerContainer.AddThemeStyleboxOverride("panel", styleBox12);
	}

	private void RunReadyAtGame()
	{
		InitializeScene();
			
		Theme theme = GD.Load<Theme>("res://game/logic/themes/Main Theme.tres");
		this.Theme = InitializeTheme(theme, "IconButton");

		StyleBox stylebox = this.Theme.GetStylebox("enabled", "IconButton");
		stateLayerContainer.AddThemeStyleboxOverride("panel", stylebox);
		if (this.Disabled)
		{
			SetDisabled();
		}
		else
		{
			InitializeButtonSignalsHandlers();
		}	
	}

	private void InitializeButtonSignalsHandlers(){
		this.MouseEntered += MouseEnteredHandler;
		this.MouseExited += MouseExitedHandler;
		this.ButtonDown += ButtonDownHandler;
		this.ButtonUp += ButtonUpHandler;
		this.FocusEntered += FocusEnteredHandler;
		this.FocusExited += FocusExitedHandler;
	}

	private void InitializeScene(){
		this.SelfModulate = new Color();
		this.FocusMode = FocusModeEnum.None;

		this.Size = new Vector2(iconButtonSize, iconButtonSize);

		icon.Texture = _texture;
		MarginContainer marginContainer = new MarginContainer();
		
		this.AddChild(marginContainer);
		this.AddChild(stateLayerContainer);

		stateLayerContainer.Size = new Vector2(stateLayerContainerSize, stateLayerContainerSize);

		stateLayerContainer.SetAnchorAndOffset(Side.Left, 0.5f, -stateLayerContainerSize/2, true);
		stateLayerContainer.SetAnchorAndOffset(Side.Top, 0.5f, -stateLayerContainerSize/2, true);
		stateLayerContainer.SetAnchorAndOffset(Side.Right, 0.5f, stateLayerContainerSize/2, true);
		stateLayerContainer.SetAnchorAndOffset(Side.Bottom, 0.5f, stateLayerContainerSize/2, true);
		
		stateLayerContainer.MouseFilter = MouseFilterEnum.Ignore;

		int marginsValue = 8;
		marginContainer.AddThemeConstantOverride("margin_top", marginsValue);
		marginContainer.AddThemeConstantOverride("margin_left", marginsValue);
		marginContainer.AddThemeConstantOverride("margin_bottom", marginsValue);
		marginContainer.AddThemeConstantOverride("margin_right", marginsValue);

		marginContainer.SetAnchorAndOffset(Side.Left, 0.5f, 0, true);
		marginContainer.SetAnchorAndOffset(Side.Top, 0.5f, 0, true);
		marginContainer.SetAnchorAndOffset(Side.Right, 0.5f, 0, true);
		marginContainer.SetAnchorAndOffset(Side.Bottom, 0.5f, 0, true);

		marginContainer.AddChild(icon);

		marginContainer.GrowHorizontal = GrowDirection.Both;
		marginContainer.GrowVertical = GrowDirection.Both;
	}
	private Theme InitializeTheme(Theme inputTheme, string inputIhemeType)
	{
		Theme theme = inputTheme;
		string themeType = inputIhemeType;

		theme.SetConstant("alphaValueForDisabled", themeType, 38);
		
		// theme.InitializeThemeColors(themeType);
		
		Dictionary<string, Color> colorsForTheme = new Dictionary<string, Color>
		{
			{"enabled", new Color()},
			{"hovered", Globals.colorsDictionary["opacityPrimary8"]},
			{"focused", Globals.colorsDictionary["opacityPrimary12"]},
			{"pressed", Globals.colorsDictionary["opacityPrimary12"]},
			{"disabled", new Color()},
		};

		InitializeStyleBoxes(theme, themeType, colorsForTheme);

		return theme;
	}

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
		styleBox.BgColor = color;
		styleBox = AllCornerRadiuses(styleBox, 100);
		return styleBox;
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

		StyleBoxFlat styleBox;
		for (int i = 0; i < numberOfStyleboxes; i++)
		{	
			string styleBoxName = styleboxNames[i];

			styleBox = InitializeSingleStylebox(colors[styleBoxName]); //probably write try-catch block
			theme.SetStylebox(styleboxNames[i], themeType, styleBox);
		}

		inputTheme = theme;
	}

	private void SetDisabled()
	{

		StyleBox stylebox = this.Theme.GetStylebox("disabled", "IconButton");
		stateLayerContainer.AddThemeStyleboxOverride("panel", stylebox);

		icon.SetColor(global::Icon.IconColors.Neutral900);
		
		Color iconModulate = icon.Modulate;
		iconModulate.A8 = (int) (this.Theme.GetConstant("alphaValueForDisabled", "IconButton") * 255 / 100);
		icon.Modulate = iconModulate;
	}

	private void ButtonUpHandler()
	{
		StyleBox stylebox = this.Theme.GetStylebox("enabled", "IconButton");
		if (this.IsHovered())
		{
			stylebox = this.Theme.GetStylebox("hovered", "IconButton");
		}

		stateLayerContainer.AddThemeStyleboxOverride("panel", stylebox);
	}

	private void ButtonDownHandler()
	{
		StyleBox stylebox = this.Theme.GetStylebox("pressed", "IconButton");
		stateLayerContainer.AddThemeStyleboxOverride("panel", stylebox);
	}

	private void FocusExitedHandler()
	{
		StyleBox stylebox = this.Theme.GetStylebox("enabled", "IconButton");
		stateLayerContainer.AddThemeStyleboxOverride("panel", stylebox);
	}

	private void FocusEnteredHandler()
	{
		StyleBox stylebox = this.Theme.GetStylebox("focused", "IconButton");
		stateLayerContainer.AddThemeStyleboxOverride("panel", stylebox);
	}

	private void MouseExitedHandler()
	{
		StyleBox stylebox = this.Theme.GetStylebox("enabled", "IconButton");
		stateLayerContainer.AddThemeStyleboxOverride("panel", stylebox);
	}

	private void MouseEnteredHandler()
	{
		StyleBox stylebox = this.Theme.GetStylebox("hovered", "IconButton");
		stateLayerContainer.AddThemeStyleboxOverride("panel", stylebox);
	}
}
