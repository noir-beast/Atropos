using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单一事件控制，控制和协调对话模块和解谜模块
/// </summary>
public class EventController : MonoBehaviour
{
    public int ID;
    PlayerController player;
    public CodeData codeData;
    public ConversationScript[] conversations;
    public bool clickAble;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// 开始对话
    /// </summary>
    /// <param name="player"></param>
    public void StartConversation(PlayerController player)
    {
        this.player = player;
        DialogController.instance.Show(conversations[0] , this, player.GetStep(ID));
    }

    /// <summary>
    /// 结束对话
    /// </summary>
    public void EndConversation()
    {
        player.SetinEvent(false);
    }

    public void StartCode()
    {
        player.inCoding = true;
        CodeController.instance.StartCode(codeData);
    }

    public void EndCode()
    {
        CodeController.instance.EndCode();
        if(NodeManager.instance.codeAnswer == false)
        {
            DialogController.instance.Show(DialogController.instance.dialogViewer.GetTarget(3));
        }
        foreach (var conversationOption in DialogController.instance.dialogViewer.CP.options)
        {
            if(conversationOption.startCode == NodeManager.instance.codeAnswer)
            {
                DialogController.instance.Show(conversationOption.targetId);
            }
        }
        player.inCoding = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerController player = collider.GetComponent<PlayerController>();
        this.player = player;
        if (player != null)
        {
            player.SetinEvent(true);
            player.eventController = this;
            StartConversation(player);
        }
    }
    void OnMouseUp()
    {
        if (clickAble && Input.GetKey(KeyCode.LeftControl))
        {
            player = PlayerController.instance;
            player.SetinEvent(true);
            player.eventController = this;
            StartConversation(player);
        }

    }

}
