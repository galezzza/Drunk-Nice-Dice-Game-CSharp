using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Reflection;

[Tool]

public partial class MyButton : PanelContainer
{
	public enum MyButtonStates
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


	private Color styleboxDefaultColor;
	private Color styleboxPressedColor;
	private Color styleboxDisabledColor;
	private Color fontColorDefaultColor;
	private Color fontColorPressedColor;


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
	[Export] public string LabelText
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
			styleboxDefaultColor = Globals.colorsDictionary["Primary"];
			styleboxPressedColor = Globals.colorsDictionary["Secondary"];
			styleboxDisabledColor = Globals.colorsDictionary["Neutral"];
			fontColorDefaultColor = Globals.colorsDictionary["White"];
			fontColorPressedColor = Globals.colorsDictionary["Primary"];
		}
		else
		{
			styleboxDefaultColor = Globals.colorsDictionary["Secondary"];
			styleboxPressedColor = Globals.colorsDictionary["Primary"];
			styleboxDisabledColor = Globals.colorsDictionary["Neutral"];
			fontColorDefaultColor = Globals.colorsDictionary["Primary"];
			fontColorPressedColor = Globals.colorsDictionary["White"];
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
			DisableButtonSignalsHandlers();
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

		theme.SetColor("styleboxDefaultColor", themeType, styleboxDefaultColor);
		theme.SetColor("styleboxPressedColor", themeType, styleboxPressedColor);
		theme.SetColor("styleboxDisabledColor", themeType, styleboxDisabledColor);
		theme.SetColor("fontColorDefaultColor", themeType, fontColorDefaultColor);
		theme.SetColor("fontColorPressedColor", themeType, fontColorPressedColor);
	}


	private void InitializeStyleBoxes(Theme inputTheme, string inputIhemeType)
	{	
		Theme theme = inputTheme;
		string themeType = inputIhemeType;

		StyleBoxFlat styleBox;

		Color styleboxDefaultColor = theme.GetColor("styleboxDefaultColor", themeType);
		Color styleboxPressedColor = theme.GetColor("styleboxPressedColor", themeType);
		Color styleboxDisabledColor = theme.GetColor("styleboxDisabledColor", themeType);

		styleBox = InitializeSingleStylebox(styleboxDefaultColor);
		theme.SetStylebox("enabled", themeType, styleBox);	

		styleBox = InitializeSingleStylebox(styleboxDefaultColor);
		StyleboxSetShadow_1(styleBox);
		theme.SetStylebox("hovered", themeType, styleBox);

		styleBox = InitializeSingleStylebox(styleboxDefaultColor);
		theme.SetStylebox("focused", themeType, styleBox);

		styleBox = InitializeSingleStylebox(styleboxPressedColor);
		theme.SetStylebox("pressed", themeType, styleBox);
		
		Color disabledStyleboxColor = styleboxDisabledColor;
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

	protected void UpdateMyButtonState(MyButtonStates state)
	{	
		string stringState = StateEnumToString(state);
		StyleBox stylebox = this.Theme.GetStylebox(stringState, "MyButton");
		this.AddThemeStyleboxOverride("panel", stylebox);

		ChangeIconAndLabelColor(state);
	}


	private void ChangeIconAndLabelColor(MyButtonStates state)
	{
		Color color = this.Theme.GetColor("fontColorDefaultColor", "MyButton");
		if (state == MyButtonStates.Pressed){
			color = this.Theme.GetColor("fontColorPressedColor", "MyButton");
		}
		
		label.AddThemeColorOverride("font_color", color);
		iconLeft.Modulate = color;
		iconRight.Modulate = color;
	}

	private void InitializeButtonSignalsHandlers(){
		Godot.Collections.Array<Godot.Collections.Dictionary> connections 
					= button.GetSignalConnectionList("mouse_entered");
		bool condition = connections.Count > 0;
		
		if (!condition)
		{
			button.MouseEntered += MouseEnteredHandler;
			button.MouseExited += MouseExitedHandler;
			button.ButtonDown += ButtonDownHandler;
			button.ButtonUp += ButtonUpHandler;
			button.FocusEntered += FocusEnteredHandler;
			button.FocusExited += FocusExitedHandler;
		}
		
	}

	private void DisableButtonSignalsHandlers()
	{	
		Godot.Collections.Array<Godot.Collections.Dictionary> connections 
					= button.GetSignalConnectionList("mouse_entered");
		bool condition = connections.Count > 0;
		
		if (condition)
		{
			button.MouseEntered -= MouseEnteredHandler;
			button.MouseExited -= MouseExitedHandler;
			button.ButtonDown -= ButtonDownHandler;
			button.ButtonUp -= ButtonUpHandler;
			button.FocusEntered -= FocusEnteredHandler;
			button.FocusExited -= FocusExitedHandler;
		}
		
	}

	protected virtual void ButtonUpHandler()
	{
		UpdateMyButtonState(MyButtonStates.Enabled);
		if (button.IsHovered())
		{
			UpdateMyButtonState(MyButtonStates.Hovered);
		}
	}

	protected virtual void ButtonDownHandler()
	{
		UpdateMyButtonState(MyButtonStates.Pressed);
	}

	protected virtual void FocusExitedHandler()
	{
		UpdateMyButtonState(MyButtonStates.Enabled);
	}

	protected virtual void FocusEnteredHandler()
	{
		UpdateMyButtonState(MyButtonStates.Focused);
	}

	protected virtual void MouseExitedHandler()
	{
		UpdateMyButtonState(MyButtonStates.Enabled);
	}

	protected virtual void MouseEnteredHandler()
	{
		UpdateMyButtonState(MyButtonStates.Hovered);
	}

	//To do 
	//1)Maybe crete current-state property
	//-On set will call UpdateMyButtonState()
	//2)Maybe separate theme and structure to different classes???
	//-MyButton look kinda big, mb thats would help
	//-But 
	//3)Create string property that will have "MyButton" -- themeType
	//-Thats will help to use separate themeType for children without overriding it
	//4)Use theme as global???
	//5)What about create toggle base class that will use for toggleButton, checkbox, radioButton and switch??
	//6)Rebase MyButton and ToggleButton from Panel Container to Button
}
