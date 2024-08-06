namespace Specter.Terminal.UI.Components;


/// <summary>
/// The root component of a UI app.
/// All other components should inherit direct
/// or indirectly from this Component.
/// </summary>
public class RootComponent : SectionComponent
{
    public RootComponent() : base("Root", null, Point.None, UI.Size.None)
    {
        PropertiesManager.SetPropertiesCanBeInherited(false);
        DrawBorder.Value = false;
    }


    public override void Update()
    {
        // * setting the Size before calling base.Update() (consequently, updating the childs)
        // * is needed to avoid drawing issues when the terminal gets resized.

        if (TerminalAttributes.TerminalResized)
            Size.Value = TerminalAttributes.TerminalSize;

        base.Update();
    }
}
