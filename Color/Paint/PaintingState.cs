namespace Specter.Color.Paint;


public struct PaintingState(ColorObject color)
{
    public ColorObject Color { get; set; } = color;
    public ColorObject DefaultColor { get; set; } = ColorValue.Reset;

    public TokenTarget? PaintUntilToken { get; set; } = null;
    public int PaintLength { get; set; }
    public int DefaultPaintLength { get; set; } = 1;
    public int PaintCounter { get; private set; } = 0;
    public bool PaintCounterIsCounting { get; private set; } = false;

    public readonly bool ShouldIgnoreRuleMatching =>
        PaintUntilToken is not null || PaintCounterIsCounting;

    public bool IgnoreCurrentToken { get; set; } = false;


    public void Update(Token currentToken)
    {
        if (IgnoreCurrentToken)
        {
            IgnoreCurrentToken = false;
            return;
        }

        if (PaintUntilToken is not null)
        {
            if (!PaintUntilToken.Value.Match(currentToken))
                return;

            PaintLength = PaintUntilToken.Value.Set.Length;
        }

        PaintUntilToken = null;

        if (++PaintCounter < PaintLength)
        {
            PaintCounterIsCounting = true;
            return;
        }

        ResetState();
    }

    public void FullReset()
    {
        PaintUntilToken = null;
        IgnoreCurrentToken = false;

        ResetState();
    }

    private void ResetState()
    {
        PaintCounterIsCounting = false;
        PaintCounter = 0;
        PaintLength = DefaultPaintLength;

        Color = DefaultColor;
    }
}
