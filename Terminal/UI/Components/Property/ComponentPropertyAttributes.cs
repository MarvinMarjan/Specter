namespace Specter.Terminal.UI.Components.Property;


public class ComponentPropertyAttributes
{
    /// <summary>
    /// Should DefaultValue be setted immediately to Value when
    /// it changes?
    /// </summary>
    public bool UpdateOnChange { get; set; } = false;

    public bool RequestOwnerRenderOnPropertyChange { get; set; } = false;
    public bool DrawAllRequest { get; set; } = false;

    public bool IgnoreManagerRequirement { get; set; } = false;
}


public class InheritableComponentPropertyAttributes : ComponentPropertyAttributes
{
    public bool Inherit { get; set; } = true;
    public bool CanBeInherited { get; set; } = true;
}
