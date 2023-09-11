using TMPro;
using UnityEngine;

/// <summary>
/// 控制节点部分的UI以及管理12个节点的执行，管理输出输出节点，检查解谜结果
/// </summary>
public class NodeManager : MonoBehaviour
{

    public static NodeManager instance = null;
    public SingleNode[] nodes;
    /// <summary>
    /// 记录哪个节点的第几行出错了
    /// </summary>
    public int[] errors;
    /// <summary>
    /// 错误总数，为0时表示可以运行
    /// </summary>
    public int errorsNum;
    public InputNode[] inputNodes;
    public OutputNode[] outputNodes;
    public bool isRuning = false;
    //public bool isSingleRuning = false;
    public bool codeAnswer;
    public bool runOver = false;
    public bool isRunOneOrder = false;
    CodeData CD;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        errors = new int[12];
        for (int i = 0; i < 12; i++)
        {
            errors[i] = -1;
        }
    }

    public void Init(CodeData CD)
    {
        for (int j = 0; j < 12; j++)
        {
            nodes[j].Init();
            errors[j] = -1;
        }
        errorsNum = 0;
        isRuning = false;
        //isSingleRuning = false;
        codeAnswer = false;
        runOver = false;
        isRunOneOrder = false;
        this.CD = CD;
        int i = 0;
        int name1 = 'A';
        foreach (var IN in inputNodes)
        {
            int[] temp;
            switch(i)
            {
                case 0:
                    temp = CD.inData0;
                    break;
                case 1:
                    temp = CD.inData1;
                    break;
                case 2:
                    temp = CD.inData2;
                    break;
                default:
                    temp = CD.inData3;
                    break;
            }
            if (i < CD.inputChoose.Length && IN.id == CD.inputChoose[i])
            {
                IN.gameObject.SetActive(true);
                IN.SetInputNode(temp, (char)(name1+i));
                i++;
            }
            else
                IN.gameObject.SetActive(false);
        }

        i = 0;
        foreach (var OUT in outputNodes)
        {
            int[] temp;
            switch (i)
            {
                case 0:
                    temp = CD.outData0;
                    break;
                case 1:
                    temp = CD.outData1;
                    break;
                case 2:
                    temp = CD.outData2;
                    break;
                default:
                    temp = CD.outData3;
                    break;
            }
            if (i < CD.outputChoose.Length && OUT.id == CD.outputChoose[i])
            {
                OUT.gameObject.SetActive(true);
                OUT.SetOutputNode((char)(name1 + i + CD.inputChoose.Length), temp.Length, temp);
                i++;
            }
            else
                OUT.gameObject.SetActive(false);
        }

        i = 0;
        foreach (var OUT in nodes)
        {
            if (i < CD.nodeUNChoose.Length && OUT.id == CD.nodeUNChoose[i])
            {
                OUT.gameObject.GetComponent<NewInput>().enabled = false;
                i++;
            }
            else
                OUT.gameObject.GetComponent<NewInput>().enabled = true;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //a = transform.Find("OutText").GetComponent<OutputNode>();
        //if(a)
        //{
        //    Debug.Log("添加out成功");
        //}
    }

    // Update is called once per frame
    void Update()
    {
        bool overFlage = true;
        if(isRuning || isRunOneOrder)
        {
            foreach(var ON in outputNodes)
            {
                if (ON.runAble && ON.iterator != ON.maxLength)
                    overFlage = false;
            }
            if(overFlage)//如果得到全部输出结果
            {
                runOver = true;
                SuspendButton();
                foreach (var ON in outputNodes)
                {
                    if (ON.runAble)
                    {
                        overFlage = ON.CheckAnswer();
                    }
                }
                codeAnswer = overFlage;
            }
            else//没有得到全部结果，还需要继续执行
            {
                if (!isRuning && !isRunOneOrder)
                    return;
                if (isRunOneOrder)
                    isRunOneOrder = false;
                foreach (var node in nodes)
                {
                    if (node.runAble)
                        node.CheckOrder();
                }
                foreach (var node in nodes)
                {
                    if (node.runAble)
                        node.RunOnce();
                }
            }
        }
    }


    public void StartButton()
    {
        if(errorsNum == 0 && isRuning == false)
        {
            if(runOver)
            {
                codeAnswer = false;
                for (int i = 0; i < 12; i++)
                {
                    if (i < 4 && outputNodes[i].runAble)
                        outputNodes[i].ReSet();
                    if (i < 4 && inputNodes[i].runAble)
                        inputNodes[i].ReSet();
                    if (nodes[i].runAble)
                    {
                        nodes[i].ReSet();
                    }
                }
            }
            runOver = false;
            isRuning = true;
            isRunOneOrder = true;
            //isSingleRuning = false;
            for (int i = 0; i < 12; i++)
            {
                if (i < 4 && outputNodes[i].runAble)
                    outputNodes[i].StartRun();
                if (i < 4 && inputNodes[i].runAble)
                    inputNodes[i].StartRun();
                if (nodes[i].runAble)
                {
                    nodes[i].StartRun();
                }
            }
        }
    }

    public void SuspendButton()
    {
        if(isRuning == true)
        {
            isRuning = false;
            for (int i = 0; i < 12; i++)
            {
                if (i < 4 && outputNodes[i].runAble)
                    outputNodes[i].Suspend();
                if (i < 4 && inputNodes[i].runAble)
                    inputNodes[i].Suspend();
                if (nodes[i].runAble == true)
                {
                    nodes[i].Suspend();
                }
            }
        }
    }

    public void SingleStepButton()
    {
        //isRunOneOrder = true;
        if (errorsNum == 0)
        {
            isRuning = false;
            isRunOneOrder = true;
            //isSingleRuning = true;
            if (runOver)
            {
                runOver = false;
                codeAnswer = false;
                for (int i = 0; i < 12; i++)
                {
                    if (i < 4 && outputNodes[i].runAble)
                        outputNodes[i].ReSet();
                    if (i < 4 && inputNodes[i].runAble)
                        inputNodes[i].ReSet();
                    if (nodes[i].runAble)
                    {
                        nodes[i].ReSet();
                    }
                }
            }
            for (int i = 0; i < 12; i++)
            {
                if (i < 4 && outputNodes[i].runAble)
                    outputNodes[i].SingleStep();
                if (i < 4 && inputNodes[i].runAble)
                    inputNodes[i].SingleStep();
                if (nodes[i].runAble == true)
                {
                    nodes[i].SingleStep();
                }
            }
        }
    }

    public void StopButton()
    {
        //if (isRuning == true && !runOver)
        {
            isRuning = false;
            for (int i = 0; i < 12; i++)
            {
                if (i < 4 && outputNodes[i].runAble)
                {
                    outputNodes[i].Stop();
                    outputNodes[i].ReSet();
                }
                if (i < 4 && inputNodes[i].runAble)
                {
                    inputNodes[i].Stop();
                    inputNodes[i].ReSet();
                }
                if (nodes[i].runAble == true)
                {
                    nodes[i].Stop();
                    nodes[i].ReSet();
                }
            }
        }
    }

    public void CloseButton()
    {
        PlayerController.instance.eventController.EndCode();
    }

    public void setOutLogText(TextMeshProUGUI T)
    {
        for (int i = 0; i < 4; i++)
        {
            if (outputNodes[i].runAble && outputNodes[i].Logtext == null)
            {
                outputNodes[i].Logtext = T;
                return;
            }
        }
    }
}
