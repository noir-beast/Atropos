using System;
using TMPro;

public class NewInput : TMP_InputField
{

    protected override void Append(char input)
    {
        if (input == '\n' && m_TextComponent.textInfo.lineCount == lineLimit)
        {
                return;
        }
        if (System.Text.RegularExpressions.Regex.IsMatch(input.ToString(), @"[A-Z\n\s:-]") || Char.IsNumber(input))
            base.Append(input);
        else if (Char.IsLower(input))
        {
            base.Append(Char.ToUpper(input));
        }
    }
    public string GetLine(int lineIdx)
    {
        if (lineIdx < 0)
            lineIdx = 0;
        if (lineIdx >= m_TextComponent.textInfo.lineCount)
            lineIdx = m_TextComponent.textInfo.lineCount - 1;
        string line = m_TextComponent.text.Substring(m_TextComponent.textInfo.lineInfo[lineIdx].firstCharacterIndex, m_TextComponent.textInfo.lineInfo[lineIdx].characterCount - 1);
        return line;
    }
    public int GetLinesCount()
    {
        return m_TextComponent.textInfo.lineCount;
    }
    public void SelectOneLine(int lineIdx)
    {
        int startPos = m_TextComponent.textInfo.lineInfo[lineIdx].firstCharacterIndex;
        int endPos = m_TextComponent.textInfo.lineInfo[lineIdx].lastCharacterIndex;
        if (startPos > endPos)
        {
            int temp = startPos;
            startPos = endPos;
            endPos = temp;
        }
        stringSelectPositionInternal = endPos ;
        stringPositionInternal = startPos;
        //SendOnFocus();

        ActivateInputField();
    }
}
