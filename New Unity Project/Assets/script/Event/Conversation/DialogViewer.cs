using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogViewer : MonoBehaviour
{
    public List<Button> buttons;
    public InputField input;
    public string answer;
    public Text text;
    public ConversationPiece CP;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 根据conversationPiece显示对话
    /// </summary>
    /// <param name="conversationPiece"></param>
    public void Show(ConversationPiece conversationPiece)
    {
        CP = conversationPiece;
        text.text = conversationPiece.text;
        foreach(var b in buttons)
        {
            b.gameObject.SetActive(false);
        }
        for(int i = 0; i < conversationPiece.options.Count; i++)
        {
            buttons[i].gameObject.SetActive(true);
            string t = conversationPiece.options[i].text;
            buttons[i].gameObject.GetComponentInChildren<Text>().text = t;
        }

        if (conversationPiece.options.Count > 0 && conversationPiece.options[0].answer != "")
        {
            this.answer = conversationPiece.options[0].answer;
            input.gameObject.SetActive(true);
        } else
        {
            input.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// <para>根据按钮ID返回下一句对话的ID，如果本次选择需要正确的输入，则只有输入正确才能返回下一句ID，否则对话不变，并在输入框的Placeholder显示输入错误</para>
    /// <para>如果这句对话没有选项，则会返回本句ID的下一个数字顺序ID 例：本句A5 返回A6 本句S8 返回S9</para>
    /// </summary>
    /// <param name="ID">按钮ID</param>
    /// <returns></returns>
    public string GetTarget(int ID)
    {
        if(CP.options.Count == 0 || ID > 2)
        {
            string targetId = "";
            targetId += CP.id[0];
            int num = int.Parse(CP.id.Substring(1));
            targetId += num+1;
            return targetId;
        }
        if(ID == 0 && CP.options[0].answer != "")
        {
            if (input.text == answer)
                return CP.options[ID].targetId;
            else
            {
                input.text = "";
                input.gameObject.GetComponentInChildren<Text>().text = CP.options[1].answer;
                return null;
            }
        }
        if (ID == 1 && CP.options.Count < 2)
            return null;
        return CP.options[ID].targetId;
    }

    public bool IsEnd()
    {
        return CP.isEnd;
    }
    public bool IsHid()
    {
        return CP.isHid;
    }

    public bool IsCode(int a)
    {
        if (a < CP.options.Count && CP.options[a].startCode)
            return true;
        return false;
    }

}
