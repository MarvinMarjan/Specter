﻿using System;


namespace Specter.Terminal.UI.Components;


/// <summary>
/// The root component of a UI app.
/// All other components should inherit direct
/// or indirectly from this Component.
/// </summary>
public class RootComponent : SectionComponent
{
	public RootComponent() : base(null, drawBorder: false)
	{
		SetAllPropertiesCanBeInherited(false);

		// * need to set this at the constructor because the first drawing
		// * frame will need it.
		//Size.DefaultValue = Terminal.GetTerminalSize();
	}


	public override void Update()
	{
		// * setting the Size before updating the base (consequently, updating the childs)
		// * is needed to avoid drawing issues when the terminal gets resized.

		if (Terminal.TerminalResized)
			Size.DefaultValue = Terminal.GetTerminalSize();
		
		base.Update();
	}
}
