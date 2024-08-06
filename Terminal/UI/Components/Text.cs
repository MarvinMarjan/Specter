using System.Text;

using Specter.ANSI;
using Specter.Terminal.UI.Components.Property;


namespace Specter.Terminal.UI.Components;


/// <summary>
/// Represents a text component.
/// * Note: Can't be defined as parent of another Component.
/// </summary>
public class TextComponent : Component, IChildLess
{
    public ComponentProperty<string> Text { get; }


    public TextComponent(string name, Component? parent, Point position, string text)
        : base(name, parent, position, UI.Size.None)
    {
        Text = new(this, "Text", text)
        {
            RequestRenderOnValueChange = true,
            DrawAllRequest = true
        };
    }


    /// <returns> A Size object based on Text. </returns>
    protected Size SizeFromText() => new(Text.Value.Length, 1);


    public override string Draw()
    {
        StringBuilder builder = new();

        builder.Append(ControlCodes.CursorTo(RelativePosition.Row, RelativePosition.Column));
        builder.Append(Color.Value.AsSequence());

        builder.Append(Text);

        builder.Append(EscapeCodes.Reset);


        builder.Append(base.Draw());

        return builder.ToString();
    }

    public override void Update()
    {
        Size.Value = SizeFromText();

        base.Update();
    }
}
