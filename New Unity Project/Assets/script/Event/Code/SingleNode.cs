using UnityEngine;
using TMPro;

public class SingleNode : MonoBehaviour
{
    public int id;

    public SingleNode up = null;
    public SingleNode right = null;
    public SingleNode down = null;
    public SingleNode left = null;
    public static string grammar0 = "^-?[1-9]{1}[0-9]{0,2}$|^0$";
    public static string grammar1 = "^ACC$|^ANY$|^UP$|^LEFT$|^RIGHT$|^DOWN$|^NIL$";
    public TextMeshProUGUI accText;
    public TextMeshProUGUI bakText;
    public TextMeshProUGUI lastText;
    public TextMeshProUGUI modeText;
    public TextMeshProUGUI upText;
    public TextMeshProUGUI rightText;
    public TextMeshProUGUI downText;
    public TextMeshProUGUI leftText;
    public TextMeshProUGUI errorAndLine;

    /// <summary>
    /// 为真时本轮才会执行命令
    /// </summary>
    public bool checkOrder;

    private int acc;
    private int bak;
    /// <summary>
    /// 八位从头开始为顺时针读写标志位 第一位第二位为，上读上写。以此类推。标志位用于判断节点要执行语句还是等待读写
    /// </summary>
    private byte nodeState;
    private OrderLine[] orderLines;
    private int linesCount = 0;

    public NewInput inputField = null;
    /// <summary>
    /// 暂停重启时标志。此节点不能使用时为false,每个被编辑过的节点会设自己为true，不可用节点（被隐藏的）不会被编辑，所以也不会为true
    /// </summary>
    public bool runAble = false;
    /// <summary>
    /// 用在update中，作为是否执行语句的判断
    /// </summary>
    public bool isRuning = false;
    public bool isRunOneOrder = false;
    private int runingOrder = 0;
    /// <summary>
    /// 要向哪个位置写就赋值给对应的
    /// </summary>
    public int forUP = -1000;
    public int forRIGHT = -1000;
    public int forDOWN = -1000;
    public int forLEFT = -1000;



    // Start is called before the first frame update
    void Start()
    {
        if(inputField) inputField.onEndEdit.AddListener(CheckAll);
        orderLines = new OrderLine[15];
    }

    public void Init()
    {
        inputField.text = "";
        ReSet();

}
    public void RunOnce()
    {
        if (!checkOrder)
            return;
        if (isRuning)
        {
            RunOrder();
        }
        else if(isRunOneOrder)
        {
            isRunOneOrder = false;
            RunOrder();
        }
    }
    /// <summary>
    /// 将输入转换成指令，并检查标签，检查语法
    /// </summary>
    /// <param name="e"></param>
    public void CheckAll(string e)
    {
        runAble = true;
        runingOrder = 0;
        //Debug.Log("检查----");
        int i = 0;
        linesCount = inputField.GetLinesCount();
        //Debug.Log("行数" + LinesCount);
        while (i < linesCount)
        {
            string oo = inputField.GetLine(i);
            //   Debug.Log("重新设置命令");
            orderLines[i] = OrderLine.GetOrderLine(oo);
            if(orderLines[i] == null)
            {
                if(NodeManager.instance.errors[id] == -1)
                {
                    NodeManager.instance.errorsNum++;
                }
                NodeManager.instance.errors[id] = i;
                ShowError(i);
                return;
            }
            i++;
        }
        int er0 = CheckLabel();
        if(er0 != -1)
        {
            if (NodeManager.instance.errors[id] == -1)
            {
                NodeManager.instance.errorsNum++;
            }
            NodeManager.instance.errors[id] = er0;
            ShowError(er0);
            return;
        }
        int er1 = CheckGrammar();
        if (er1 != -1)
        {
            if (NodeManager.instance.errors[id] == -1)
            {
                NodeManager.instance.errorsNum++;
            }
            NodeManager.instance.errors[id] = er1;
            ShowError(er1);
            return;
        }

        if (NodeManager.instance.errors[id] != -1)
        {
            NodeManager.instance.errorsNum--;
            NodeManager.instance.errors[id] = -1;
        }
        if(linesCount == 1 && orderLines[0].isNull())
        {
            runAble = false;
            ShowNGS();
        }
        else
            ShowNoError();
        //if(runAble)
        //{
        //    isRuning = true;
        //    down.isRuning = true;
        //    Debug.Log("开始");
        //}
        //else 
        //    Debug.Log("失败");
    }
    /// <summary>
    /// 检查语法
    /// </summary>
    /// <returns></returns>
    public int CheckGrammar()
    {
        for(int i = 0; i < linesCount; i++)
        {
            OrderLine oneLine = orderLines[i];
            if (oneLine.isNull())
                continue;
            if (oneLine.label != null && oneLine.order == null)
                continue;
            switch (oneLine.order)
            {
                case "MOV":
                    if (oneLine.src != null && oneLine.dst != null)
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(oneLine.src, grammar0+"|"+grammar1))
                            return i;
                        if (!System.Text.RegularExpressions.Regex.IsMatch(oneLine.dst, grammar1))
                            return i;
                    }
                    else
                        return i;
                    break;
                case "SWP":
                case "NOP":
                case "SAV":
                case "NEG":
                    if (oneLine.src != null || oneLine.dst != null)
                        return i;
                    break;
                case "ADD":
                case "SUB":
                case "JRO":
                    if (oneLine.src != null && oneLine.dst == null)
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(oneLine.src, grammar0 + "|" + grammar1))
                            return i;
                    }
                    else
                        return i;
                    break;
                case "JMP":
                case "JEZ":
                case "JNZ":
                case "JGZ":
                case "JLZ":
                    if (oneLine.src == null || oneLine.dst != null)
                        return i;
                    break;
                default:
                    return i;
            }
        }
        return -1;
    }
    /// <summary>
    /// 确定跳转指令的目标位置
    /// </summary>
    /// <returns>返回出错的行 如果返回-1表示无错
    /// </returns>
    public int CheckLabel()
    {
        for (int i = 0; i < linesCount; i++)
        {
            if (orderLines[i].targer != -421)
                for (int j = 0; j < linesCount; j++)
                {
                    if (i == j || orderLines[j].targer == -421)
                        continue;
                    if (orderLines[i].label == orderLines[j].src && orderLines[i].label != null)
                    {
                        orderLines[j].targer = i;
                        if(orderLines[i].targer == -666) orderLines[i].targer = -1000;
                    }
                    else
                    {
                        if (orderLines[j].label == orderLines[i].src && orderLines[j].label != null)
                        {
                            orderLines[i].targer = j;
                            if (orderLines[j].targer == -666) orderLines[j].targer = -1000;
                        }
                    }
                }
            if (orderLines[i].targer == -666)
            {
                return i;
            }
        }
        return -1;
    }

    private void RunOrder()
    {
        //if (nodeState != 0 && !TryRW())//如果正在等待读写并且本次还不能读写，就接着等待
        //{
        //    return;
        //}
        if (runingOrder == linesCount)
            runingOrder = 0;
        ShowLine(runingOrder);
        //orderLines[runingOrder].Display();
        while (orderLines[runingOrder].order == null)
        {
            runingOrder++;
            runingOrder %= linesCount;
        }

        switch (orderLines[runingOrder].order)
        {
            case "NOP":
                runingOrder++;
                break;
            case "MOV":
                MOV();
                break;
            case "SWP":
                SWP();
                break;
            case "SAV":
                SAV();
                break;
            case "ADD":
                ADD();
                break;
            case "SUB":
                SUB();
                break;
            case "NEG":
                NEG();
                break;
            case "JMP":
                JMP();
                break;
            case "JEZ":
                JEZ();
                break;
            case "JNZ":
                JNZ();
                break;
            case "JGZ":
                JGZ();
                break;
            case "JLZ":
                JLZ();
                break;
            case "JRO":
                JRO();
                break;
            default:
                break;
        }

    }

    private bool WriteANY(int src)
    {
        bool isWrite;
        if(up && up.GetType().Name != "InputNode")
        {
            isWrite = WriteUP(src);
            if (isWrite)
                return true;
        }
        isWrite = WriteRIGHT(src);
        if (isWrite)
            return true;
        isWrite = WriteDOWN(src);
        if (isWrite)
            return true;
        isWrite = WriteLEFT(src);
        if (isWrite)
            return true;
        else
            return false;

    }

    private bool WriteLEFT(int src)
    {
        if (left && forLEFT == -1000)
        {
            forLEFT = src;
            leftText.text = src.ToString();
            lastText.text = "Left";
            return true;
        }
        else
            return false;
    }

    private bool WriteDOWN(int src)
    {
        if (down && forDOWN == -1000)
        {
            forDOWN = src;
            downText.text = src.ToString();
            lastText.text = "Down";
            return true;
        }
        else
            return false;
    }

    private bool WriteRIGHT(int src)
    {
        if (right && forRIGHT == -1000)
        {
            forRIGHT = src;
            rightText.text = src.ToString();
            lastText.text = "Right";
            return true;
        }
        else
            return false;
    }

    private bool WriteUP(int src)
    {
        if (up && forUP == -1000)
        {
            forUP = src;
            upText.text = src.ToString();
            lastText.text = "UP";
            return true;
        }
        else
            return false;
    }

    private void MOV()
    {
        int src = Read(orderLines[runingOrder].src);
        if (src == -1000)
            return;
        Write(orderLines[runingOrder].dst, src);
        runingOrder++;
    }
    private void SWP()
    {
        acc ^= bak;
        bak ^= acc;
        acc ^= bak;
        accText.text = acc.ToString();
        bakText.text = bak.ToString();
        runingOrder++;
    }

    private void SAV()
    {
        bak = acc;
        bakText.text = bak.ToString();
        runingOrder++;
    }

    private void ADD()
    {
        acc += Read(orderLines[runingOrder].src);
        accText.text = acc.ToString();
        runingOrder++;
    }

    private void SUB()
    {
        acc -= Read(orderLines[runingOrder].src);
        accText.text = acc.ToString();
        runingOrder++;
    }

    private void NEG()
    {
        acc = -acc;
        accText.text = acc.ToString();
        runingOrder++;
    }

    private void JMP()
    {
        runingOrder = orderLines[runingOrder].targer;
    }
    private void JEZ()
    {
        if(acc == 0)
            runingOrder = orderLines[runingOrder].targer;
        else
            runingOrder++;
    }
    private void JNZ()
    {
        if(acc != 0)
            runingOrder = orderLines[runingOrder].targer;
        else
            runingOrder++;
    }
    private void JGZ()
    {
        if (acc > 0)
            runingOrder = orderLines[runingOrder].targer;
        else
            runingOrder++;
    }
    private void JLZ()
    {
        if (acc < 0)
            runingOrder = orderLines[runingOrder].targer;
        else
            runingOrder++;
    }

    private void JRO()
    {
        runingOrder += Read(orderLines[runingOrder].src);
    }


    private bool TryRW()
    {
        switch(nodeState)
        {
            case 0b00000000://any读
                {
                    if (up && up.forDOWN != -1000)
                        return true;
                    if (right && right.forLEFT != -1000)
                        return true;
                    if (down && down.forUP != -1000)
                        return true;
                    if (left && left.forRIGHT != -1000)
                        return true;
                }
                return true;
            case 0b00000001://上读
                if(up && up.forDOWN != -1000)
                    return true;
                break;
            case 0b00000010://上写
                if(forUP == -1000)
                    return true;
                break;
            case 0b00000100://右读
                if(right && right.forLEFT != -1000)
                    return true;
                break;
            case 0b00001000://右写
                if (forRIGHT == -1000)
                    return true;
                break;
            case 0b00010000://下读
                if (down && down.forUP != -1000)
                    return true;
                break;
            case 0b00100000://下写
                if (forDOWN == -1000)
                    return true;
                break;
            case 0b01000000://左读
                if (left && left.forRIGHT != -1000)
                    return true;
                break;
            case 0b10000000://左写
                if (forLEFT == -1000)
                    return true;
                break;
            case 0b11111111://any写
                if (forUP == -1000)
                    return true;
                if (forRIGHT == -1000)
                    return true;
                if (forDOWN == -1000)
                    return true;
                if (forLEFT == -1000)
                    return true;
                break;
        }
        return false;
    }

    private int ReadUP()
    {
        if (up)
        {
            int temp = up.forDOWN;
            up.forDOWN = -1000;
            if (up.GetType() == GetType())
            {
                //up.downText.SetText("");
                //up.downText.GetComponentInChildren<TextMeshProUGUI>().SetText("↓");
                up.downText.text = "";
                lastText.text = "Up";
            }
            return temp;
        }
        else return -1000;
    }

    private int ReadRIGHT()
    {
        if (right)
        {
            int temp = right.forLEFT;
            right.forLEFT = -1000;
            right.leftText.text = " ";
            lastText.text = "Rright";
            return temp;
        }
        else return -1000;
    }

    private int ReadDOWN()
    {
        if (down)
        {
            int temp = down.forUP;
            down.forUP = -1000;
            down.upText.text = " ";
            lastText.text = "Down";
            return temp;
        }
        else return -1000;
    }

    private int ReadLEFT()
    {
        if (left)
        {
            int temp = left.forRIGHT;
            left.forRIGHT = -1000;
            left.rightText.text = " ";
            lastText.text = "Left";
            return temp;
        }
        else return -1000;
    }

    private int ReadANY()
    {
        int num;
        num = ReadUP();
        if (num != -1000)
            return num;
        num = ReadRIGHT();
        if (num != -1000)
            return num;
        if (down && down.GetType().Name != "OutputNode")
        {
            num = ReadDOWN();
            if (num != -1000)
                return num;
        }
        num = ReadLEFT();
        if (num != -1000)
            return num;
        else
            return -1000;
    }

    public virtual void StartRun()
    {
        isRuning = true;
    }

    public virtual void Suspend()
    {
        isRuning = false;
    }
     public virtual void SingleStep()
    {
        isRuning = false;
        isRunOneOrder = true;
    }

    public virtual void Stop()
    {
        isRuning = false;
        runingOrder = 0;
        forUP = -1000;
        forRIGHT = -1000;
        forDOWN = -1000;
        forLEFT = -1000;
    }

    public virtual void ReSet()
    {
        runingOrder = 0;
        forUP = -1000;
        forRIGHT = -1000;
        forDOWN = -1000;
        forLEFT = -1000;
        acc = 0;
        bak = 0;
        if(accText)
        {
            accText.text = "";
            bakText.text = "";
            lastText.text = "";
            modeText.text = "";
            upText.text = "";
            rightText.text = "";
            downText.text = "";
            leftText.text = "";
        }
    }

    public void ShowLine(int index)
    {
        errorAndLine.color = Color.white;
        errorAndLine.text = (index+1).ToString();
    }
    public void ShowError(int index)
    {
        errorAndLine.color = Color.red;
        errorAndLine.text = (index + 1).ToString();
    }

    public void ShowNoError()
    {
        errorAndLine.color = Color.green;
        errorAndLine.text = "No\nError";
    }
    public void ShowNGS()
    {
        errorAndLine.color = Color.white;
        errorAndLine.text = "MADE\nBY\nNGS";
    }
    public void ShowUnavaiable()
    {
        errorAndLine.color = Color.white;
        errorAndLine.text = "CAN'T\nUSE";
    }

    public void CheckOrder()
    { 
        if (runingOrder >= linesCount)
            runingOrder = 0;
        while (orderLines[runingOrder].order == null)
        {
            runingOrder++;
            runingOrder %= linesCount;
        }
        checkOrder = false;
        switch (orderLines[runingOrder].src)//判断是否可读
        {
            case "UP":
                nodeState = 1 << 0;
                break;
            case "RIGHT":
                nodeState = 1 << 2;
                break;
            case "DOWN":
                nodeState = 1 << 4;
                break;
            case "LEFT":
                nodeState = 1 << 6;
                break;
            case "ANY":
                nodeState = 0b11111111;
                break;
            default://向acc写
                checkOrder = true;
                break;
        }
        if (!checkOrder && !TryRW())
        {
            modeText.text = "read";
            return;
        }
        checkOrder = false;
        switch (orderLines[runingOrder].dst)//判断是否可写
        {
            case "UP":
                nodeState = 1 << 1;
                break;
            case "RIGHT":
                nodeState = 1 << 3;
                break;
            case "DOWN":
                nodeState = 1 << 5;
                break;
            case "LEFT":
                nodeState = 1 << 7;
                break;
            case "ANY":
                nodeState = 0b11111111;
                break;
            default://向acc写
                checkOrder = true;
                break;
        }
        if (!checkOrder && !TryRW())
        {
            modeText.text = "write";
            return;
        }
        modeText.text = "run";
        checkOrder = true;
    }

    int Read(string target)
    {
        switch (target)
        {
            case "UP":
                return ReadUP();
            case "RIGHT":
                return ReadRIGHT();
            case "DOWN":
                return ReadDOWN();
            case "LEFT":
                return ReadLEFT();
            case "ACC":
                return acc;
            case "ANY":
                return ReadANY();
            case "NIL":
                return 0;
            default:
                return int.Parse(orderLines[runingOrder].src);
        }
    }
    void Write(string target,int src)
    {
        switch (target)//写需要返回值，bool判断是否可写，src当参数 返回true就说明写上了，否则等待
        {
            case "UP":
                WriteUP(src);//应该是write 
                break;
            case "RIGHT":
                WriteRIGHT(src);
                break;
            case "DOWN":
                WriteDOWN(src);
                break;
            case "LEFT":
                WriteLEFT(src);
                break;
            case "ACC":
                acc = src;
                accText.text = acc.ToString();
                break;
            case "ANY":
                WriteANY(src);
                break;
            case "NIL":
                break;
            default:
                //抛出异常？
                break;
        }
    }

}
