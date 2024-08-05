using Specter.ANSI;


namespace Specter.Terminal;


/// <summary>
/// Defines methods that use ANSI control codes for controlling the terminal behavior.
/// </summary>
public static class ControlCodes
{
    public enum ScreenErasingMode
    {
        CursorUntilEnd,
        CursorUntilBeginning,
        Full,
        SavedLines
    }


    public enum LineErasingMode
    {
        CursorUntilEnd,
        CursorUntilBeginning,
        Full
    }


    public static string CursorToHome()
        => EscapeCodes.EscapeCodeWithController + 'H';
    
    public static string CursorTo(int line, int column)
        => $"{EscapeCodes.EscapeCodeWithController}{line};{column}H";

    public static string CursorUp(int lines)
        => $"{EscapeCodes.EscapeCodeWithController}{lines}A";

    public static string CursorDown(int lines)
        => $"{EscapeCodes.EscapeCodeWithController}{lines}B";

    public static string CursorRight(int columns)
        => $"{EscapeCodes.EscapeCodeWithController}{columns}C";

    public static string CursorLeft(int columns)
        => $"{EscapeCodes.EscapeCodeWithController}{columns}D";

    public static string CursorToBeginningOfPreviousLine(int lines)
        => $"{EscapeCodes.EscapeCodeWithController}{lines}E";

    public static string CursorToBeginningOfNextLine(int lines)
        => $"{EscapeCodes.EscapeCodeWithController}{lines}F";

    public static string CursorToColumn(int lines)
        => $"{EscapeCodes.EscapeCodeWithController}{lines}G";

    public static string SaveCursorPos()
        => $"{EscapeCodes.EscapeCodeWithController}s";

    public static string LoadCursorPos()
        => $"{EscapeCodes.EscapeCodeWithController}u";



    public static string EraseScreen(ScreenErasingMode mode = ScreenErasingMode.Full)
        => $"{EscapeCodes.EscapeCodeWithController}{(int)mode}J";
    public static string EraseEntireScreen()
        => $"{EscapeCodes.EscapeCodeWithController}J";
    public static string EraseLine(LineErasingMode mode = LineErasingMode.Full)
        => $"{EscapeCodes.EscapeCodeWithController}{(int)mode}K";
}
