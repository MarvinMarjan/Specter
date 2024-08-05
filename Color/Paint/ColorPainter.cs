namespace Specter.Color.Paint;


/// <summary>
/// A Painter for ColorObjects.
/// </summary>
/// <param name="color"> The ColorObject to use. </param>
public class ColorPainter(ColorObject color) : Painter
{
    public ColorObject Color { get; set; } = color;


    public override string Paint(string source)
        => Color.AsSequence() + source + SequenceFinisher;
}
