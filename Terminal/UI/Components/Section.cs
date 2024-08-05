using System.Text;

using Specter.ANSI;
using Specter.Color;
using Specter.Color.Paint;
using Specter.Terminal.UI.Components.Property;


namespace Specter.Terminal.UI.Components;


/// <summary>
/// Component that represents a section.
/// </summary>
public class SectionComponent : Component
{

    // Component properties

    public InheritableComponentProperty<char> BackgroundFill { get; }
    public InheritableComponentProperty<BorderCharacters> BorderCharacters { get; }
    public InheritableComponentProperty<ColorObject> BorderColor { get; }
    public InheritableComponentProperty<bool> DrawBorder { get; }


    public SectionComponent(
        string name,

        Component? parent,
        Point? position = null,
        Size? size = null,

        Alignment? alignment = null,

        ColorObject? color = null,

        bool inheritProperties = true,

        BorderCharacters? borderCharacters = null,
        ColorObject? borderColor = null,
        bool drawBorder = true,
        char backgroundFill = ' '


    ) : base(name, parent, position, size, alignment, color, inheritProperties)
    {
        BorderCharacters = new(
            this, "BorderCharacters", borderCharacters ?? UI.BorderCharacters.Default,
            Parent?.As<SectionComponent>()?.BorderCharacters,

            new InheritableComponentPropertyAttributes { RequestOwnerRenderOnPropertyChange = true }
        );

        BorderColor = new(
            this, "BorderColor", borderColor ?? Color,
            Parent?.As<SectionComponent>()?.BorderColor,

            new InheritableComponentPropertyAttributes { RequestOwnerRenderOnPropertyChange = true }
        )
        {
            LinkProperty = Color,
            UseLink = true // * if you want to set a different color to border, disable UseLink first.
        };

        DrawBorder = new(
            this, "DrawBorder", drawBorder,
            Parent?.As<SectionComponent>()?.DrawBorder,

            new InheritableComponentPropertyAttributes { RequestOwnerRenderOnPropertyChange = true }
        );

        BackgroundFill = new(
            this, "BackgroundFill", backgroundFill,
            Parent?.As<SectionComponent>()?.BackgroundFill,

            new InheritableComponentPropertyAttributes { RequestOwnerRenderOnPropertyChange = true }
        );
    }


    protected void DrawAt(ref StringBuilder builder, int row, int col)
    {
        // draws the border
        if (DrawBorder && Bounds.IsAtBorder(new Point(row, col) + Position, out Bounds.Edge edges))
        {
            ColorPainter painter = new(BorderColor) { SequenceFinisher = Color.Value.AsSequence() };
            string characterAsString = new(BorderCharacters.Value.GetBorderCharFromEdgeFlags(edges), 1);

            builder.Append(painter.Paint(characterAsString));
            return;
        }

        // draws the background
        builder.Append(BackgroundFill);
    }


    public override string Draw()
    {
        StringBuilder builder = new();

        builder.Append(ControlCodes.CursorTo(RelativePosition.Row, RelativePosition.Column));
        builder.Append(Color.Value.AsSequence());

        for (int i = 0; i < Size.Value.Height; i++)
        {
            for (int o = 0; o < Size.Value.Width; o++)
                DrawAt(ref builder, i, o);

            builder.Append(ControlCodes.CursorDown(1) + ControlCodes.CursorToColumn(RelativePosition.Column));
        }

        builder.Append(EscapeCodes.Reset);


        // draw childs
        builder.Append(base.Draw());

        return builder.ToString();
    }
}
