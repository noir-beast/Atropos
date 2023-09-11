using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct ConversationPiece
{
    public string id;
    [Multiline]
    public bool isEnd;
    public bool isHid;
    public bool savePoint;
    public string saveTargetID;
    public string text;
    public List<ConversationOption> options;
}
