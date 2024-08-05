using Specter.ANSI;


namespace Specter.Color;


/// <summary>
/// The layer of a color. Color elements like Color256 or ColorRGB
/// should use this enum to define which layer they represents.
/// </summary>
public enum ColorLayer
{
    Foreground = 38,
    Background = 48
}


/// <summary>
/// A color element for Color16.
/// </summary>
/// <param name="code"> The color code. </param>
public class ColorCodeElement(Color16? code = null) : SequenceElement
{
    public Color16? Code { get; set; } = code;


    public override bool IsValid() => Code is not null;

    protected override string BuildSequence()
        => ((int)Code!).ToString();


    public static implicit operator ColorCodeElement(Color16 code) => new(code);
}


/// <summary>
/// A color element for 8-bit (0-255) coloring.
/// 
/// Take a look at https://gist.github.com/fnky/458719343aabd01cfb17a3a4f7296797#colors--graphics-mode
/// </summary>
/// <param name="code"> The 8-bit color code. </param>
/// <param name="layer"> The layer of the element. </param>
public class Color256Element(byte? code = null, ColorLayer layer = ColorLayer.Foreground) : SequenceElement
{
    public byte? Code { get; set; } = code;
    public ColorLayer Layer { get; set; } = layer;


    public override bool IsValid() => Code is not null;

    protected override string BuildSequence()
        => SequenceBuilder.BuildEscapeSequence([
            ((int)Layer).ToString(), EscapeCodes.Color256TypeCode.ToString(), Code!.ToString()
        ], false);
    

    public static implicit operator Color256Element(byte code) => new(code);
}


/// <summary>
/// A color element for RGB based coloring.
/// </summary>
/// <param name="color"> The RGB color. </param>
/// <param name="layer"> The layer of the element. </param>
public class ColorRGBElement(ColorRGB? color = null, ColorLayer layer = ColorLayer.Foreground) : SequenceElement
{
    public ColorRGB? Color { get; set; } = color;
    public ColorLayer Layer { get; set; } = layer;


    public override bool IsValid() => Color is not null;

    protected override string BuildSequence()
    {
        ColorRGB validColor = Color!.Value;

        if (!validColor.AreAllChannelsNull())
            validColor.SetValueToNullChannels(0);

        return SequenceBuilder.BuildEscapeSequence([
            ((int)Layer).ToString(), EscapeCodes.ColorRGBTypeCode.ToString(),
            validColor.Red?.ToString(), validColor.Green?.ToString(), validColor.Blue?.ToString()
        ], false);
    }


    public static implicit operator ColorRGBElement(ColorRGB color) => new(color);
}
