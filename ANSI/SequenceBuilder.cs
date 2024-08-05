using System.Linq;
using System.Text;


namespace Specter.ANSI;



/// <summary>
/// Interface for representing a sequence element.
/// At the sequence "\x1b[0;31;45m", the numbers "0", "31" and "45" are
/// elements.
/// </summary>
public abstract class SequenceElement
{
    /// <summary>
    /// Checks whether a sequence element is valid or not.
    /// </summary>
    /// <param name="element"> The element to check. </param>
    /// <returns> True if it's valid, false otherwise. </returns>
    public static bool IsValid(SequenceElement? element)
        => element is not null && element.IsValid();


    /// <summary>
    /// Checks whether the current object is valid or not.
    /// </summary>
    /// <returns> True if it's valid, false otherwise </returns>
    public abstract bool IsValid();


    // ! Calling this method when IsValid() is false may result in a exception.
    protected abstract string BuildSequence();

    /// <summary>
    /// Builds the sequence as a string.
    /// </summary>
    /// <returns> A string containing the sequence. </returns>
    public string BuildSequenceIfValid()
    {
        if (!IsValid())
            return string.Empty;

        return BuildSequence();
    }
}


public static class SequenceBuilder
{
    public static string AddEscapeCode(string source)
    {
        source = source.Insert(0, EscapeCodes.EscapeCodeWithController);
        source += EscapeCodes.EscapeCodeEnd;

        return source;
    }

    public static void AddEscapeCode(ref StringBuilder source)
    {
        source.Insert(0, EscapeCodes.EscapeCodeWithController);
        source.Append(EscapeCodes.EscapeCodeEnd);
    }


    /// <summary>
    /// Builds an ANSI sequence based on codes of an array.
    /// i.e: the input ["1", "34", "102"] generates "\x1b[1;34;102m"
    /// </summary>
    /// <param name="codes"> The array containing the codes. </param>
    /// <param name="useEscapeCode"> Should use escape codes in the string? </param>
    public static string BuildEscapeSequence(string?[] codes, bool useEscapeCode = true)
    {
        StringBuilder builder = new();

        // removes any null or empty value from "codes"
        var validCodes = (from code in codes where code is not null and not "" select code).ToArray();

        builder.Append(string.Join(';', validCodes));

        if (useEscapeCode)
            AddEscapeCode(ref builder);

        return builder.ToString();
    }
}
