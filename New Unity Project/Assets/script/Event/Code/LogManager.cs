using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 控制解谜界面的其余部分的更新和显示
/// </summary>
public class LogManager : MonoBehaviour
{
    public static LogManager instance = null;
    public TextMeshProUGUI[] logs;
    public TextMeshProUGUI[] names;
    public Text message;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {

    }

    public void Init(CodeData CD)
    {
        foreach (var nameText in names)
        {
            nameText.text = "";
        }
        foreach (var logText in logs)
        {
            logText.text = "";
        }
        int[][] inData = new int[CD.inputChoose.Length][];
        int[][] outData = new int[CD.outputChoose.Length][];
        int i = 0;
        int name1 = 'A';
        while (i < CD.inputChoose.Length)
        {
            switch (i)
            {
                case 0:
                    names[i].text = "A";
                    SetData(logs[0], CD.inData0);
                    break;
                case 1:
                    names[i].text = "B";
                    SetData(logs[1], CD.inData1);
                    break;
                case 2:
                    names[i].text = "C";
                    SetData(logs[2], CD.inData2);
                    break;
                case 3:
                    names[i].text = "D";
                    SetData(logs[3], CD.inData3);
                    break;
            }
            i++;
        }

        i = 0;
        while (i < CD.outputChoose.Length)
        {
            name1 = 'A' + CD.inputChoose.Length + i;
            char name2 = (char)name1;
            names[CD.inputChoose.Length + i * 2].text = name2.ToString();
            names[CD.inputChoose.Length + i * 2 + 1].text = name2.ToString().ToLower();
            switch (i)
            {
                case 0:
                    SetData(logs[CD.inputChoose.Length + i * 2], CD.outData0);
                    NodeManager.instance.setOutLogText(logs[CD.inputChoose.Length + i * 2 + 1]);
                    break;
                case 1:
                    SetData(logs[CD.inputChoose.Length + i * 2], CD.outData1);
                    NodeManager.instance.setOutLogText(logs[CD.inputChoose.Length + i * 2 + 1]);
                    break;
                case 2:
                    SetData(logs[CD.inputChoose.Length + i * 2], CD.outData2);
                    NodeManager.instance.setOutLogText(logs[CD.inputChoose.Length + i * 2 + 1]);
                    break;
                case 3:
                    SetData(logs[CD.inputChoose.Length + i * 2], CD.outData3);
                    NodeManager.instance.setOutLogText(logs[CD.inputChoose.Length + i * 2 + 1]);
                    break;
            }
            i++;
        }

        message.text = CD.message;
    }

    void SetData(TextMeshProUGUI T, int[] D)
    {
        T.text = "";
        int i = 0;
        while (i < D.Length)
        {
            T.text += D[i].ToString();
            i++;
            if (i < D.Length)
                T.text += "\n";
        }
    }

}
