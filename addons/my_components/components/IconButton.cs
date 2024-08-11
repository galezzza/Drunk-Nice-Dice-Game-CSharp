using Godot;
using System;

public partial class IconButton : Button
{

	private Control stateLayerContainer;

	public override void _Ready()
	{
		stateLayerContainer = GetNode<PanelContainer>("%State-layer container");
		StyleBox stylebox = this.Theme.GetStylebox("enabled", "IconButton");
		stateLayerContainer.AddThemeStyleboxOverride("panel", stylebox);


		stateLayerContainer.MouseFilter = MouseFilterEnum.Ignore;
	}


}
