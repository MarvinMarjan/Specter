using System;


namespace Specter.Terminal.UI;


public struct TerminalUnit
{
    private int _value;
    public int Value
    {
        readonly get => _value;
        set
        {
            if (value >= 0)
                _value = value;
            else
                throw new InvalidOperationException(@"A ""TerminalUnit"" value is not allowed to be negative.");
        }
    }


    public TerminalUnit(int value)
    {
        Value = value;
    }


    public static implicit operator TerminalUnit(int value) => new(value);
    public static implicit operator int(TerminalUnit value) => value.Value;
}


/// <summary>
/// Represents a point in the terminal.
/// </summary>
/// <param name="row"> The row position. </param>
/// <param name="col"> The column position. </param>
public struct Point(int row, int col)
{
    public static Point None { get => new(0, 0); }


    public TerminalUnit Row { get; set; } = row;
    public TerminalUnit Column { get; set; } = col;



    public static Point operator +(Point left, Point right)
        => new(left.Row + right.Row, left.Column + right.Column);

    public static Point operator -(Point left, Point right)
        => new(left.Row - right.Row, left.Column - right.Column);

    public static Point operator *(Point left, Point right)
        => new(left.Row * right.Row, left.Column * right.Column);

    public static Point operator /(Point left, Point right)
        => new(left.Row / right.Row, left.Column / right.Column);


    public static bool operator ==(Point left, Point right)
        => left.Equals(right);

    public static bool operator !=(Point left, Point right)
        => !left.Equals(right);


    public readonly override int GetHashCode() => (Row, Column).GetHashCode();

    public readonly override bool Equals(object? obj)
        => obj is Point point && Equals(point);

    public readonly bool Equals(Point obj) => Row == obj.Row && Column == obj.Column;


    public override readonly string ToString()
        => $"Pos(row: {Row}, col: {Column})";
}


/// <summary>
/// Represents a size in the terminal.
/// </summary>
/// <param name="width"> The width. </param>
/// <param name="height"> The height. </param>
public struct Size(int width, int height)
{

    public static Size None { get => new(0, 0); }


    public TerminalUnit Width { get; set; } = width;
    public TerminalUnit Height { get; set; } = height;


    public static Size operator +(Size left, Size right)
        => new(left.Width + right.Width, left.Height + right.Height);

    public static Size operator -(Size left, Size right)
        => new(left.Width - right.Width, left.Height - right.Height);

    public static Size operator *(Size left, Size right)
        => new(left.Width * right.Width, left.Height * right.Height);

    public static Size operator /(Size left, Size right)
        => new(left.Width / right.Width, left.Height / right.Height);


    public static bool operator ==(Size left, Size right)
        => left.Equals(right);

    public static bool operator !=(Size left, Size right)
        => !left.Equals(right);


    public readonly override int GetHashCode() => (Width, Height).GetHashCode();

    public readonly override bool Equals(object? obj)
        => obj is Size size && Equals(size);

    public readonly bool Equals(Size obj) => Width == obj.Width && Height == obj.Height;


    public readonly override string ToString()
        => $"Size(width: {Width}, height: {Height})";
}


/// <summary>
/// Encapsulates both position and size in a single object.
/// </summary>
/// <param name="position"> The position. </param>
/// <param name="size"> The size. </param>
public struct Rect(Point position, Size size)
{
    public Point Position { get; set; } = position;
    public Size Size { get; set; } = size;
}


/// <summary>
/// Represents the bounds of a rectangle in the terminal.
/// </summary>
/// <param name="top"> The top. </param>
/// <param name="left"> The left. </param>
/// <param name="bottom"> The bottom. </param>
/// <param name="right"> The right. </param>
public struct Bounds(int top, int left, int bottom, int right)
{
    public TerminalUnit Top { get; set; } = top;
    public TerminalUnit Left { get; set; } = left;
    public TerminalUnit Bottom { get; set; } = bottom;
    public TerminalUnit Right { get; set; } = right;


    [Flags]
    public enum Edge
    {
        None = 0b0000,

        Top = 0b0001,
        Left = 0b0010,
        Bottom = 0b0100,
        Right = 0b1000,

        TopLeft = Top | Left,
        TopRight = Top | Right,
        BottomLeft = Bottom | Left,
        BottomRight = Bottom | Right
    }


    public static Bounds FromRectangle(Point position, Size size)
        => new(position.Row, position.Column, position.Row + size.Height - 1, position.Column + size.Width - 1);


    public static bool HasEdgeInEdges(Edge edges, Edge edge)
        => (edges & edge) == edge;


    public readonly bool IsAtBorder(Point point)
        => (point.Row == Top || point.Row == Bottom) && point.Column >= Left && point.Column <= Right ||
            (point.Column == Left || point.Column == Right) && point.Row >= Top && point.Row <= Bottom;


    public readonly bool IsAtBorder(Point point, out Edge edges)
    {
        edges = 0;

        if (!IsAtBorder(point))
        {
            edges = 0;
            return false;
        }

        edges |= point.Row == Top ? Edge.Top : 0;
        edges |= point.Row == Bottom ? Edge.Bottom : 0;
        edges |= point.Column == Left ? Edge.Left : 0;
        edges |= point.Column == Right ? Edge.Right : 0;

        return true;
    }
}
