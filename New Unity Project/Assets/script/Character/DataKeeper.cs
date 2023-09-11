using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// 游戏进度管理模块，在切换场景前数据会被保存在这里，进入新场景后首先从这里读取数据
/// </summary>
public class DataKeeper : MonoBehaviour
{
    public static DataKeeper instance;
    /// <summary>
    /// 场景名字
    /// </summary>
    public string sceneName;
    /// <summary>
    /// 角色坐标
    /// </summary>
    public Vector3 playerPosition;
    /// <summary>
    /// 每个场景中记录每个对话树的进度
    /// </summary>
    public string[][] eventStepKey;
    /// <summary>
    /// 每个场景中记录每个挂载了事件的对象是否显示
    /// </summary>
    public bool[][] eventFlage;
    /// <summary>
    /// 每个场景中记录每个挂载了事件的对象是否显示
    /// </summary>
    public Vector2 lookDirection = new Vector2(0, -1);
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void KeepData(PlayerController player)
    {
        sceneName = SceneManager.GetActiveScene().name;
        playerPosition = player.transform.position;
        eventStepKey = player.eventStepKey;
        eventFlage = player.eventFlage;
        lookDirection = player.lookDirection;
    }
    public void KeepData(SaveData saveDate)
    {
        sceneName = saveDate.sceneName;
        playerPosition = new Vector3(saveDate.playerPositionX, saveDate.playerPositionY, saveDate.playerPositionZ);
        eventStepKey = saveDate.eventStepKey;
        eventFlage = saveDate.eventFlage;
        lookDirection = new Vector3(saveDate.lookDirectionX, saveDate.lookDirectionY);
    }

    public void InitData()
    {
        sceneName = "0";
        playerPosition = new Vector3(-2.5f,-0.5f);
        eventStepKey = new string[6][];
        eventFlage = new bool[6][];
        for (int i = 0; i < 6; i++)
        {
            eventStepKey[i] = new string[10];
            eventFlage[i] = new bool[10];
            for (int j = 0; j < 10; j++)
            {
                eventFlage[i][j] = true;
            }
        }
        lookDirection = new Vector2(0, -1);
    }

}
