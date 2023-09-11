using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制解谜模式的节点管理器和日志管理器
/// </summary>
public class CodeController : MonoBehaviour
{
    public NodeManager nodeManager;
    public LogManager logManager;
    public static CodeController instance;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
    public void StartCode(CodeData codeData)
    {
        nodeManager.gameObject.SetActive(true);
        logManager.gameObject.SetActive(true);
        nodeManager.Init(codeData);
        logManager.Init(codeData);
    }
    public void EndCode()
    {
        nodeManager.gameObject.SetActive(false);
        logManager.gameObject.SetActive(false);
    }
}
