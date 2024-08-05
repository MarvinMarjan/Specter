namespace Specter.Color;


/// <summary>
/// Pre-defined ColorObjects for all Color16 codes.
/// </summary>
public static class ColorValue
{
    public static ColorObject Reset => ColorObject.FromColor16(fg: Color16.Reset);


    public static ColorObject FGBlack   => ColorObject.FromColor16(fg: Color16.FGBlack);
    public static ColorObject FGRed     => ColorObject.FromColor16(fg: Color16.FGRed);
    public static ColorObject FGGreen   => ColorObject.FromColor16(fg: Color16.FGGreen);
    public static ColorObject FGYellow  => ColorObject.FromColor16(fg: Color16.FGYellow);
    public static ColorObject FGBlue    => ColorObject.FromColor16(fg: Color16.FGBlue);
    public static ColorObject FGMagenta => ColorObject.FromColor16(fg: Color16.FGMagenta);
    public static ColorObject FGCyan    => ColorObject.FromColor16(fg: Color16.FGCyan);
    public static ColorObject FGWhite   => ColorObject.FromColor16(fg: Color16.FGWhite);
    public static ColorObject FGDefault => ColorObject.FromColor16(fg: Color16.FGDefault);


    public static ColorObject BGBlack   => ColorObject.FromColor16(bg: Color16.BGBlack);
    public static ColorObject BGRed     => ColorObject.FromColor16(bg: Color16.BGRed);
    public static ColorObject BGGreen   => ColorObject.FromColor16(bg: Color16.BGGreen);
    public static ColorObject BGYellow  => ColorObject.FromColor16(bg: Color16.BGYellow);
    public static ColorObject BGBlue    => ColorObject.FromColor16(bg: Color16.BGBlue);
    public static ColorObject BGMagenta => ColorObject.FromColor16(bg: Color16.BGMagenta);
    public static ColorObject BGCyan    => ColorObject.FromColor16(bg: Color16.BGCyan);
    public static ColorObject BGWhite   => ColorObject.FromColor16(bg: Color16.BGWhite);
    public static ColorObject BGDefault => ColorObject.FromColor16(bg: Color16.BGDefault);


    public static ColorObject FGBBlack   => ColorObject.FromColor16(fg: Color16.FGBBlack);
    public static ColorObject FGBRed     => ColorObject.FromColor16(fg: Color16.FGBRed);
    public static ColorObject FGBGreen   => ColorObject.FromColor16(fg: Color16.FGBGreen);
    public static ColorObject FGBYellow  => ColorObject.FromColor16(fg: Color16.FGBYellow);
    public static ColorObject FGBBlue    => ColorObject.FromColor16(fg: Color16.FGBBlue);
    public static ColorObject FGBMagenta => ColorObject.FromColor16(fg: Color16.FGBMagenta);
    public static ColorObject FGBCyan    => ColorObject.FromColor16(fg: Color16.FGBCyan);
    public static ColorObject FGBWhite   => ColorObject.FromColor16(fg: Color16.FGBWhite);


    public static ColorObject BGBBlack   => ColorObject.FromColor16(bg: Color16.BGBBlack);
    public static ColorObject BGBRed     => ColorObject.FromColor16(bg: Color16.BGBRed);
    public static ColorObject BGBGreen   => ColorObject.FromColor16(bg: Color16.BGBGreen);
    public static ColorObject BGBYellow  => ColorObject.FromColor16(bg: Color16.BGBYellow);
    public static ColorObject BGBBlue    => ColorObject.FromColor16(bg: Color16.BGBBlue);
    public static ColorObject BGBMagenta => ColorObject.FromColor16(bg: Color16.BGBMagenta);
    public static ColorObject BGBCyan    => ColorObject.FromColor16(bg: Color16.BGBCyan);
    public static ColorObject BGBWhite   => ColorObject.FromColor16(bg: Color16.BGBWhite);


    public static ColorObject Normal    => ColorObject.FromColorMode(ColorMode.Normal);
    public static ColorObject Bold      => ColorObject.FromColorMode(ColorMode.Bold);
    public static ColorObject Dim       => ColorObject.FromColorMode(ColorMode.Dim);
    public static ColorObject Italic    => ColorObject.FromColorMode(ColorMode.Italic);
    public static ColorObject Underline => ColorObject.FromColorMode(ColorMode.Underline);
    public static ColorObject Blinking  => ColorObject.FromColorMode(ColorMode.Blinking);
    public static ColorObject Inverse   => ColorObject.FromColorMode(ColorMode.Inverse);
    public static ColorObject Hidden    => ColorObject.FromColorMode(ColorMode.Hidden);
    public static ColorObject Strike    => ColorObject.FromColorMode(ColorMode.Strike);
}
