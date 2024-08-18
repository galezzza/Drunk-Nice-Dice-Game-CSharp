#if TOOLS
using Godot;
using System;

[Tool]
public partial class my_components : EditorPlugin
{
	public override void _EnterTree()
	{
		// Initialization of the plugin goes here

		Script scriptIcon = GD.Load<Script>("res://addons/my_components/components/Icon.cs");
		Texture2D texIcon = GD.Load<Texture2D>("res://addons/my_components/components/Icon.png");
		AddCustomType("Icon", "TextureRect", scriptIcon, texIcon);
		
		Script scriptIconButton = GD.Load<Script>("res://addons/my_components/components/IconButton.cs");
		Texture2D texIconButton = GD.Load<Texture2D>("res://addons/my_components/components/IconButton.png");
		AddCustomType("IconButton", "Button", scriptIconButton, texIconButton);
		
		Script scriptMyButton = GD.Load<Script>("res://addons/my_components/components/MyButton.cs");
		Texture2D texMyButton = GD.Load<Texture2D>("res://addons/my_components/components/MyButton.png");
		AddCustomType("MyButton", "PanelContainer", scriptMyButton, texMyButton);
		
		Script scriptToggleButton = GD.Load<Script>("res://addons/my_components/components/ToggleButton.cs");
		Texture2D texToggleButton = GD.Load<Texture2D>("res://addons/my_components/components/ToggleButton.png");
		AddCustomType("ToggleButton", "PanelContainer", scriptToggleButton, texToggleButton);
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.

		RemoveCustomType("Icon");
		RemoveCustomType("IconButton");
		RemoveCustomType("MyToggleButton");
		RemoveCustomType("MyButton");
		
	}
}
#endif
