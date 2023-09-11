using UnityEngine;

public class OrderLine
{
    /// <summary>
    /// 跳跃指令的目标行 label就不为空
    /// </summary>
    public string label;
    /// <summary>
    /// 指令行
    /// </summary>
    public string order;
    public string src;
    public string dst;
    /// <summary>
    /// 跳跃指令的偏移量,-666为起始或目标行，-421是与跳转无关的行，-1000或其他属于(-9,9)表示这行可跳转
    /// </summary>
    public int targer;

    OrderLine()
    {
        label = order = src = dst = null;
        targer = -421;
    }
    /// <summary>
    /// 将一行输入按顺序填到OrderLine类的属性中，如果这行输入的长度符合要求
    /// </summary>
    /// <param name="orderline"></param>
    /// <returns></returns>
    public static OrderLine GetOrderLine(string orderline)
    {
        OrderLine oneOrderLine = new OrderLine();
        orderline = orderline.Trim();
        if(string.IsNullOrEmpty(orderline))
        {
            oneOrderLine.label = oneOrderLine.order = oneOrderLine.src = oneOrderLine.dst = null;
            oneOrderLine.targer = -421;
            return oneOrderLine;
        }
        string[] orders;
        orders = orderline.Split(' ');
        if (orders[0][orders[0].Length - 1] == ':')
        {
            if (orders.Length > 4)
                return null;
            oneOrderLine.label = orders[0].Substring(0, orders[0].Length - 1);
            for (int i = 1; i < orders.Length; i++)
            {
                switch(i)
                {
                    case 1:
                        oneOrderLine.order = orders[i];
                        break;
                    case 2:
                        oneOrderLine.src = orders[i];
                        break;
                    case 3:
                        oneOrderLine.dst = orders[i];
                        break;
                }
            }
        }
        else
        {
            if (orders.Length > 3)
                return null;
            for (int i = 0; i < orders.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        oneOrderLine.order = orders[i];
                        break;
                    case 1:
                        oneOrderLine.src = orders[i];
                        break;
                    case 2:
                        oneOrderLine.dst = orders[i];
                        break;
                }
            }
        }

        if(oneOrderLine.label != null || oneOrderLine.order == "JMP" || oneOrderLine.order == "JEZ" || oneOrderLine.order == "JNZ" || oneOrderLine.order == "JGZ" || oneOrderLine.order == "JLZ")//如果这行是目标，或者是起点
        {
            oneOrderLine.targer = -666;
        }
        return oneOrderLine;
    }

    public void Display()
    {
        Debug.Log(label + " " + order + " " + src + " " + dst + " " + targer);
    }

    public bool isNull()
    {
        if (targer == -421 && label == null && order == null && src == null && dst == null)
            return true;
        return false;
    }
}
