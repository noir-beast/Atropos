using UnityEngine;
using UnityEngine.UI;
using System;

public class TISInputField : InputField
{

    protected override void Append(char input)
    {
        if (System.Text.RegularExpressions.Regex.IsMatch(input.ToString(), @"[A-Z\n\s:-]") || Char.IsNumber(input))
            base.Append(input);
        else if (Char.IsLower(input))
        {
            base.Append(Char.ToUpper(input));
        }
    }

    public string GetLine(int lineIdx)
    {
        int startPos = GetLineStartPosition(cachedInputTextGenerator, lineIdx);
        int endPos = GetLineEndPosition(cachedInputTextGenerator, lineIdx);
        if (startPos > endPos)
        {
            int temp = startPos;
            startPos = endPos;
            endPos = temp;
        }

        return text.Substring(startPos, endPos - startPos);
    }

    public int GetLinesCount()
    {
        return cachedInputTextGenerator.lineCount;
    }

    private int GetLineStartPosition(TextGenerator gen, int line)
    {
        line = Mathf.Clamp(line, 0, gen.lines.Count - 1);
        return gen.lines[line].startCharIdx;
    }

    private int GetLineEndPosition(TextGenerator gen, int line)
    {
        line = Mathf.Max(line, 0);
        if (line + 1 < gen.lines.Count)
            return gen.lines[line + 1].startCharIdx - 1;
        return gen.characterCountVisible;
    }

    public void SelectOneLine(int lineIdx)
    {
        int startPos = GetLineStartPosition(cachedInputTextGenerator, lineIdx);
        int endPos = GetLineEndPosition(cachedInputTextGenerator, lineIdx);
        if (startPos > endPos)
        {
            int temp = startPos;
            startPos = endPos;
            endPos = temp;
        }
        caretPositionInternal = startPos;
        caretSelectPositionInternal = endPos;
    }

}
