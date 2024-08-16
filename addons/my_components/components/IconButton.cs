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

	private enum IconButtonStates
	{
		Enabled,
		Hovered,
		Focused,
		Pressed,
		Disabled
	}
	private static string[] states = new string[] {"enabled", "hovered", "focused", "pressed", "disabled"};


	private Color styleboxDefaultColor;
	private Color styleboxHoverColor;
	private Color styleboxFocusColor;
	private Color styleboxPressedColor;
	private Color styleboxDisabledColor;


	private PanelContainer stateLayerContainer = new PanelContainer();
	private Icon icon = new Icon();


	private static int iconButtonSize = 48;
	private static int stateLayerContainerSize = 40;
	private static int marginsValue = 8;

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

	private bool _isDisabled = false;
	[Export] public bool IsDisabled
	{
		get => _isDisabled;
		set
		{
			_isDisabled = value;
			OnSetDisabled(value);
		}
	}
	private void OnSetDisabled(bool value)
	{
		this.Disabled = value;
		if (value == true)
		{
			UpdateIconButtonState(IconButtonStates.Disabled);
			DisableButtonSignalsHandlers();
		}
		else
		{
			UpdateIconButtonState(IconButtonStates.Enabled);
			InitializeButtonSignalsHandlers();
		}
	}

	private void OnTextureSet(Texture2D texture)
	{
		icon.Texture = texture;
	}

	public override void _Ready()
	{	
		InitializeScene();
			
		Theme theme = GD.Load<Theme>("res://game/logic/themes/Main Theme.tres");
		this.Theme = InitializeTheme(theme, "IconButton");

		UpdateIconButtonState(IconButtonStates.Enabled);
		if (IsDisabled == true)
		{
			UpdateIconButtonState(IconButtonStates.Disabled);
		}
		else
		{
			InitializeButtonSignalsHandlers();
		}

	}

	private void InitializeButtonSignalsHandlers()
	{
		Godot.Collections.Array<Godot.Collections.Dictionary> connections 
					= this.GetSignalConnectionList("MouseEntered");
		bool condition = connections.Count > 0;

		if (!condition)
		{
			this.MouseEntered += MouseEnteredHandler;
			this.MouseExited += MouseExitedHandler;
			this.ButtonDown += ButtonDownHandler;
			this.ButtonUp += ButtonUpHandler;
			this.FocusEntered += FocusEnteredHandler;
			this.FocusExited += FocusExitedHandler;
		}
		
	}
	private void DisableButtonSignalsHandlers()
	{
		Godot.Collections.Array<Godot.Collections.Dictionary> connections 
					= this.GetSignalConnectionList("MouseEntered");
		bool condition = connections.Count > 0;
		
		if (condition)
		{
			this.MouseEntered -= MouseEnteredHandler;
			this.MouseExited -= MouseExitedHandler;
			this.ButtonDown -= ButtonDownHandler;
			this.ButtonUp -= ButtonUpHandler;
			this.FocusEntered -= FocusEnteredHandler;
			this.FocusExited -= FocusExitedHandler;
		}
	}

	private void InitializeScene()
	{
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

		styleboxDefaultColor = new Color();
		styleboxHoverColor = Globals.colorsDictionary["opacityPrimary8"];
		styleboxFocusColor = Globals.colorsDictionary["opacityPrimary12"];
		styleboxPressedColor = Globals.colorsDictionary["opacityPrimary12"];
		styleboxDisabledColor = Globals.colorsDictionary["neutral900"];
	
		theme.SetColor("styleboxDefaultColor", themeType, styleboxDefaultColor);
		theme.SetColor("styleboxHoverColor", themeType, styleboxHoverColor);
		theme.SetColor("styleboxFocusColor", themeType, styleboxFocusColor);
		theme.SetColor("styleboxPressedColor", themeType, styleboxPressedColor);
		theme.SetColor("styleboxDisabledColor", themeType, styleboxDisabledColor);

		InitializeStyleBoxes(theme, themeType);

		return theme;
	}

	private void InitializeStyleBoxes(Theme inputTheme, string inputIhemeType)
	{		
		Theme theme = inputTheme;
		string themeType = inputIhemeType;

		StyleBoxFlat styleBox;

		Color styleboxDefaultColor = theme.GetColor("styleboxDefaultColor", themeType);
		Color styleboxHoverColor = theme.GetColor("styleboxHoverColor", themeType);
		Color styleboxFocusColor = theme.GetColor("styleboxFocusColor", themeType);
		Color styleboxPressedColor = theme.GetColor("styleboxPressedColor", themeType);
		Color styleboxDisabledColor = theme.GetColor("styleboxDisabledColor", themeType);

		styleBox = InitializeSingleStylebox(styleboxDefaultColor);
		theme.SetStylebox("enabled", themeType, styleBox);	

		styleBox = InitializeSingleStylebox(styleboxHoverColor);
		theme.SetStylebox("hovered", themeType, styleBox);

		styleBox = InitializeSingleStylebox(styleboxFocusColor);
		theme.SetStylebox("focused", themeType, styleBox);

		styleBox = InitializeSingleStylebox(styleboxPressedColor);
		theme.SetStylebox("pressed", themeType, styleBox);
		
		Color disabledStyleboxColor = styleboxDisabledColor;
		disabledStyleboxColor.A = 0.38f;
		styleBox = InitializeSingleStylebox(disabledStyleboxColor);
		theme.SetStylebox("disabled", themeType, styleBox);
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

	// private void SetDisabled()
	// {

	// 	UpdateMyButtonState(IconButtonStates.Disabled);

	// 	icon.SetColor(global::Icon.IconColors.Neutral900);
		
	// 	Color iconModulate = icon.Modulate;
	// 	iconModulate.A8 = (int) (this.Theme.GetConstant("alphaValueForDisabled", "IconButton") * 255 / 100);
	// 	icon.Modulate = iconModulate;
	// }

	private string StateEnumToString(IconButtonStates state)
	{
		int index = (int) state;
		return states[index];
	}

	private void UpdateIconButtonState(IconButtonStates state)
	{	
		string stringState = StateEnumToString(state);
		StyleBox stylebox = this.Theme.GetStylebox(stringState, "IconButton");
		stateLayerContainer.AddThemeStyleboxOverride("panel", stylebox);
	}

	private void ButtonUpHandler()
	{
		UpdateIconButtonState(IconButtonStates.Enabled);
		if (this.IsHovered())
		{
			UpdateIconButtonState(IconButtonStates.Hovered);
		}
	}

	private void ButtonDownHandler()
	{
		UpdateIconButtonState(IconButtonStates.Pressed);
	}

	private void FocusExitedHandler()
	{
		UpdateIconButtonState(IconButtonStates.Enabled);
	}

	private void FocusEnteredHandler()
	{
		UpdateIconButtonState(IconButtonStates.Focused);
	}

	private void MouseExitedHandler()
	{
		UpdateIconButtonState(IconButtonStates.Enabled);
	}

	private void MouseEnteredHandler()
	{
		UpdateIconButtonState(IconButtonStates.Hovered);
	}
}
