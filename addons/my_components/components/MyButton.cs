using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

[Tool]

public partial class MyButton : PanelContainer
{
	private enum MyButtonStates
	{
		Enabled,
		Hovered,
		Focused,
		Pressed,
		Disabled
	}
	private static string[] states = new string[] {"enabled", "hovered", "focused", "pressed", "disabled"};


	public enum MyButtonColors
	{
		Primary,
		Secondary,
	}


	private Color styleboxDefault;
	private Color styleboxPressed;
	private Color styleboxDisabled;
	private Color fontColorDefault;
	private Color fontColorPressed;


	private Icon iconLeft = new Icon();
	private Icon iconRight = new Icon();
	private Label label = new Label();


	private MarginContainer marginContainer = new MarginContainer();
	private HBoxContainer hBoxContainer = new HBoxContainer();
	private Button button = new Button();


	
	private Texture2D _textureIconLeft;
	[Export] public Texture2D TextureIconLeft
	{
		get => _textureIconLeft;
		set
		{
			_textureIconLeft = value;
			OnLeftIconSet(value);
		}
	}

	private Texture2D _textureIconRight;
	[Export] public Texture2D TextureIconRight
	{
		get => _textureIconRight;
		set
		{
			_textureIconRight = value;
			OnRightIconSet(value);
		}
	}

	private bool _showLeftIcon = false;
	[Export] public bool ShowLeftIcon
	{
		get => _showLeftIcon;
		set
		{
			_showLeftIcon = value;
			OnShowLeftIconSet(value);
		}
	}

	private bool _showRightIcon = false;
	[Export] public bool ShowRightIcon
	{
		get => _showRightIcon;
		set
		{
			_showRightIcon = value;
			OnShowRightIconSet(value);
		}
	}

	private string _labelText;
	[Export]
	public string LabelText
	{
		get => _labelText;
		set
		{
			_labelText = value;
			OnLabelSet(value);
		}
	}

	private MyButtonColors _myButtonColor;
	[Export] public MyButtonColors MyButtonColor
	{
		get => _myButtonColor;
		set
		{
			_myButtonColor = value;
			this.Theme = InitializeTheme(this.Theme, "MyButton");
			UpdateMyButtonState(MyButtonStates.Enabled);
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



	private int marginTopButtomValue = 10;
	private static int separationValue = 8;
	private static int marginLeftValueWithoutIcon = 24;
	private static int marginRightValueWithoutIcon = 24;
	private static int marginLeftValueWithIcon = 16;
	private static int marginRightValueWithIcon = 16;

	private int marginLeftValue = marginLeftValueWithoutIcon;
	private int marginRightValue = marginRightValueWithoutIcon;


	private void OnColorSet(MyButtonColors color)
	{	
		if (color == MyButtonColors.Primary)
		{
			styleboxDefault = Globals.colorsDictionary["Primary"];
			styleboxPressed = Globals.colorsDictionary["Secondary"];
			styleboxDisabled = Globals.colorsDictionary["Neutral"];
			fontColorDefault = Globals.colorsDictionary["White"];
			fontColorPressed = Globals.colorsDictionary["Primary"];
		}
		else
		{
			styleboxDefault = Globals.colorsDictionary["Secondary"];
			styleboxPressed = Globals.colorsDictionary["Primary"];
			styleboxDisabled = Globals.colorsDictionary["Neutral"];
			fontColorDefault = Globals.colorsDictionary["Primary"];
			fontColorPressed = Globals.colorsDictionary["White"];
		}	
	}

	private void OnLabelSet(string value)
	{
		label.Text = value;
	}
	private void OnLeftIconSet(Texture2D value)
	{
		iconLeft.Texture = value;
	}
	private void OnRightIconSet(Texture2D value)
	{
		iconRight.Texture = value;
	}
	private void OnShowLeftIconSet(bool value)
	{
		_showLeftIcon = value;
		iconLeft.Visible = _showLeftIcon;
		UpdateLeftMargins();
	}
	private void OnShowRightIconSet(bool value)
	{
	   _showRightIcon = value;
		iconRight.Visible = _showRightIcon;
		UpdateRightMargins();
	}
	private void OnSetDisabled(bool value)
	{
		button.Disabled = value;
		if (value == true)
		{
			UpdateMyButtonState(MyButtonStates.Disabled);
		}
		else
		{
			UpdateMyButtonState(MyButtonStates.Enabled);
			InitializeButtonSignalsHandlers();
		}
	}

	public override void _Ready()
	{	
		CreateNodeTree();
		InitializeNodes();

		Theme theme = GD.Load<Theme>("res://game/logic/themes/Main Theme.tres");
		this.Theme = InitializeTheme(theme, "MyButton");
		UpdateMyButtonState(MyButtonStates.Enabled);

		if (IsDisabled == true)
		{
			UpdateMyButtonState(MyButtonStates.Disabled);
		}
		else
		{
			InitializeButtonSignalsHandlers();
		}

		// if (!Engine.IsEditorHint())
		// {
		// 	// Code to execute when in game.
		// }	
	}

	private void CreateNodeTree()
	{
		hBoxContainer.AddChild(iconLeft);
		hBoxContainer.AddChild(label);
		hBoxContainer.AddChild(iconRight);
		marginContainer.AddChild(hBoxContainer);
		
		this.AddChild(marginContainer);
		this.AddChild(button);
	}

	private void InitializeNodes()
	{	
		button.SelfModulate = new Color();
		button.FocusMode = FocusModeEnum.None;
		this.FocusMode = FocusModeEnum.None;

		iconLeft.Visible = _showLeftIcon;
		iconRight.Visible = _showRightIcon;

		UpdateLeftMargins();
		UpdateRightMargins();
		UpdateMarginContainerMargins(marginContainer);

		this.MouseFilter = MouseFilterEnum.Ignore;
		MouseFilterOffIncludeChildren(marginContainer);	
		
		InitializeHBoxContainer(hBoxContainer);
	}

	private void InitializeHBoxContainer(HBoxContainer hBoxContainer)
	{
		hBoxContainer.SizeFlagsVertical = SizeFlags.ShrinkCenter;
		hBoxContainer.AddThemeConstantOverride("separation", separationValue);
	}

	private void UpdateMarginContainerMargins(MarginContainer marginContainer)
	{
		marginContainer.AddThemeConstantOverride("margin_top", marginTopButtomValue);
		marginContainer.AddThemeConstantOverride("margin_left", marginLeftValue);
		marginContainer.AddThemeConstantOverride("margin_bottom", marginTopButtomValue);
		marginContainer.AddThemeConstantOverride("margin_right", marginRightValue);
	}

	private void UpdateLeftMargins()
	{
		marginLeftValue = marginLeftValueWithoutIcon;
		if(_showLeftIcon)
		{
			marginLeftValue = marginLeftValueWithIcon;
		}
	}
	private void UpdateRightMargins()
	{
		marginRightValue = marginRightValueWithoutIcon;
		if(_showRightIcon)
		{
			marginRightValue = marginRightValueWithIcon;
		}
	}

	void MouseFilterOffIncludeChildren(Control startNode){
		List<Node> nodes = GetAllChildren(startNode);
		foreach (Control node in nodes)
		{
			node.MouseFilter = MouseFilterEnum.Ignore;
		}
	}

	private List<Node> GetAllChildren(Node inNode)
	{
		List<Node> arr = new List<Node>();

  		arr.Add(inNode);

  		foreach (Node child in inNode.GetChildren())
		{
			arr = GetAllChildren(child);
		}

		return arr;
	}

	private Theme InitializeTheme(Theme inputTheme, string inputIhemeType)
	{
		Theme theme = inputTheme;
		string themeType = inputIhemeType;

		InitializeThemeColors(theme, themeType);
		InitializeStyleBoxes(theme, themeType);

		return theme;
	}

	private void InitializeThemeColors(Theme inputTheme, string inputIhemeType){
		Theme theme = inputTheme;
		string themeType = inputIhemeType;

		OnColorSet(this.MyButtonColor);

		theme.SetColor("styleboxDefault", themeType, styleboxDefault);
		theme.SetColor("styleboxPressed", themeType, styleboxPressed);
		theme.SetColor("styleboxDisabled", themeType, styleboxDisabled);
		theme.SetColor("fontColorDefault", themeType, fontColorDefault);
		theme.SetColor("fontColorPressed", themeType, fontColorPressed);
	}


	private void InitializeStyleBoxes(Theme inputTheme, string inputIhemeType)
	{	
		Theme theme = inputTheme;
		string themeType = inputIhemeType;

		StyleBoxFlat styleBox;

		Color primary = theme.GetColor("styleboxDefault", themeType);
		Color secondary = theme.GetColor("styleboxPressed", themeType);
		Color neutral = theme.GetColor("styleboxDisabled", themeType);

		styleBox = InitializeSingleStylebox(primary);
		theme.SetStylebox("enabled", themeType, styleBox);	

		styleBox = InitializeSingleStylebox(primary);
		StyleboxSetShadow_1(styleBox);
		theme.SetStylebox("hovered", themeType, styleBox);

		styleBox = InitializeSingleStylebox(primary);
		theme.SetStylebox("focused", themeType, styleBox);

		styleBox = InitializeSingleStylebox(secondary);
		theme.SetStylebox("pressed", themeType, styleBox);
		
		Color disabledStyleboxColor = neutral;
		disabledStyleboxColor.A = 0.38f;
		styleBox = InitializeSingleStylebox(disabledStyleboxColor);
		theme.SetStylebox("disabled", themeType, styleBox);
	}

	private void StyleboxSetShadow_1(StyleBoxFlat styleBox){
		styleBox.ShadowColor = new Color(0, 0, 0, 0.3f);
		styleBox.ShadowOffset = new Vector2(1, 1);
		styleBox.ShadowSize = 2;
	}

	private StyleBoxFlat AllCornerRadiuses(StyleBoxFlat styleBox, int value)
	{
		styleBox.CornerRadiusBottomLeft = value;
		styleBox.CornerRadiusBottomRight = value;
		styleBox.CornerRadiusTopLeft = value;
		styleBox.CornerRadiusTopRight = value;
		return styleBox;
	}

	private StyleBoxFlat InitializeSingleStylebox(Color color)
	{	
		StyleBoxFlat styleBox = new StyleBoxFlat();
		styleBox.BgColor = color;
		styleBox = AllCornerRadiuses(styleBox, 100);
		return styleBox;
	}

	private string StateEnumToString(MyButtonStates state)
	{
		int index = (int) state;
		return states[index];
	}

	private void UpdateMyButtonState(MyButtonStates state)
	{	
		string stringState = StateEnumToString(state);
		StyleBox stylebox = this.Theme.GetStylebox(stringState, "MyButton");
		this.AddThemeStyleboxOverride("panel", stylebox);

		ChangeIconAndLabelColor(state);
	}


	private void ChangeIconAndLabelColor(MyButtonStates state)
	{
		Color color = this.Theme.GetColor("fontColorDefault", "MyButton");
		if (state == MyButtonStates.Pressed){
			color = this.Theme.GetColor("fontColorPressed", "MyButton");
		}
		
		label.AddThemeColorOverride("font_color", color);
		iconLeft.Modulate = color;
		iconRight.Modulate = color;
	}

	private void InitializeButtonSignalsHandlers(){
		button.MouseEntered += MouseEnteredHandler;
		button.MouseExited += MouseExitedHandler;
		button.ButtonDown += ButtonDownHandler;
		button.ButtonUp += ButtonUpHandler;
		button.FocusEntered += FocusEnteredHandler;
		button.FocusExited += FocusExitedHandler;
	}

	private void ButtonUpHandler()
	{
		UpdateMyButtonState(MyButtonStates.Enabled);
		if (button.IsHovered())
		{
			UpdateMyButtonState(MyButtonStates.Hovered);
		}
	}

	private void ButtonDownHandler()
	{
		UpdateMyButtonState(MyButtonStates.Pressed);
	}

	private void FocusExitedHandler()
	{
		UpdateMyButtonState(MyButtonStates.Enabled);
	}

	private void FocusEnteredHandler()
	{
		UpdateMyButtonState(MyButtonStates.Focused);
	}

	private void MouseExitedHandler()
	{
		UpdateMyButtonState(MyButtonStates.Enabled);
	}

	private void MouseEnteredHandler()
	{
		UpdateMyButtonState(MyButtonStates.Hovered);
	}
}
