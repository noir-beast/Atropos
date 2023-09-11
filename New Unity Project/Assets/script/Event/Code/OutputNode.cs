using TMPro;
using UnityEngine;
/// <summary>
/// 接收输出，判断解谜结果
/// </summary>
public class OutputNode : SingleNode
{

    public int[] outputData;
    public int[] answerData;
    public int iterator = 0;
    public int maxLength = 0;
    public TextMeshProUGUI Logtext;
    public TextMeshProUGUI nameText;


    public void SetOutputNode(char name, int maxLength, int[] answerData)
    {
        this.answerData = answerData;
        nameText.text = name.ToString();
        this.maxLength = maxLength;
        outputData = new int[maxLength];
        runAble = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isRuning)
        {
            GetData();
        }
        else if (isRunOneOrder)
        {
            isRunOneOrder = false;
            GetData();
        }
    }

    public void GetData()
    {
        if (up != null && up.forDOWN != -1000 && iterator < maxLength)
        {
            outputData[iterator] = up.forDOWN;
            if(Logtext) Logtext.text +=  up.forDOWN.ToString();
            up.forDOWN = -1000;
            iterator++;
            if(iterator != maxLength && Logtext)
                Logtext.text += '\n';
        }
        if(iterator == maxLength)
        {
            isRuning = false;

        }
    }

    public bool CheckAnswer()
    {
        int i = 0;
        while(i < answerData.Length)
        {
            if (answerData[i] != outputData[i])
            {
                Logtext.color = Color.red;
                return false;
            }
            i++;
        }
        Logtext.color = Color.green;
        return true;
    }

    public override void Stop()
    {
        if (Logtext)
        {
            Logtext.text = "";
            Logtext.color = Color.white;
        }
         iterator = 0;
        base.Stop();
    }

    public override void ReSet()
    {
        if (Logtext)
        {
            Logtext.text = "";
            Logtext.color = Color.white;
        }
        iterator = 0;
        for (int i = 0; i < maxLength; i++)
        {
            outputData[i] = -1000;
        }
        base.ReSet();
    }
}
