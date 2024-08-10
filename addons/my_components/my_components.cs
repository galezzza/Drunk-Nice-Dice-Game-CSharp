#if TOOLS
using Godot;
using System;

[Tool]
public partial class my_components : EditorPlugin
{
	public override void _EnterTree()
	{
		// Initialization of the plugin goes here

		Script script = GD.Load<Script>("res://addons/my_components/components/Icon.cs");
		Texture2D tex = GD.Load<Texture2D>("res://addons/my_components/components/Icon.png");
		

		AddCustomType("Icon", "TextureRect", script, tex);
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.

		RemoveCustomType("Icon");
	}
}
#endif
