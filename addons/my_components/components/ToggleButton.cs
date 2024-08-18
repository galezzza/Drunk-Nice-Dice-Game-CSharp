using Godot;
using System;
[Tool]

public partial class ToggleButton : MyButton
{
	private bool _isPressed = false;
	[Export] public bool IsPressed
	{
		get => _isPressed;
		set
		{
			_isPressed = value;
			OnSetPressed(value);
		}
	}   
	private void OnSetPressed(bool value)
	{
	   
	}
	
	protected override void ButtonUpHandler()
	{
		
	}

	protected override void ButtonDownHandler()
	{   
		if (IsPressed)
		{
			UpdateMyButtonState(MyButtonStates.Hovered);
			IsPressed = false;
		}
		else
		{
			UpdateMyButtonState(MyButtonStates.Pressed);
			IsPressed = true;
		}
	}
	protected override void MouseExitedHandler()
	{
		if (IsPressed)
		{
			// UpdateMyButtonState(MyButtonStates.Enabled);
		}
		else
		{
			UpdateMyButtonState(MyButtonStates.Enabled);
		}
	}

	protected override void MouseEnteredHandler()
	{
		if (IsPressed)
		{
			// UpdateMyButtonState(MyButtonStates.Enabled);
		}
		else
		{
			UpdateMyButtonState(MyButtonStates.Hovered);
		}
	}
}
