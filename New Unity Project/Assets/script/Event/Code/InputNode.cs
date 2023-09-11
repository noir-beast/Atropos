using TMPro;
/// <summary>
/// 提供输入
/// </summary>
public class InputNode : SingleNode
{

    public int[] inputData;
    public int iterator = 0;
    public TextMeshProUGUI text;
    public char Name;

    public void SetInputNode(int[] inputData, char name)
    {
        this.inputData = inputData;
        this.Name = name;
        text.text = name.ToString();
        runAble = true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(isRuning)
        {
            PushData();
        }
        else if (isRunOneOrder)
        {
            isRunOneOrder = false;
            PushData();
        }
    }

    public void PushData()
    {
        if (down != null && forDOWN == -1000 && iterator < inputData.Length)
        {
            forDOWN = inputData[iterator++];
            text.text = Name + ":" + forDOWN.ToString();
        }
        else if(forDOWN == -1000 && iterator == inputData.Length)
        {
            text.text = Name + ":null";
            isRuning = false;
        }
    }


    public override void Stop()
    {
        iterator = 0;
        base.Stop();
    }

    public override void ReSet()
    {
        iterator = 0;
        forDOWN = -1000;
        text.text = name;
        base.ReSet();
    }
}
