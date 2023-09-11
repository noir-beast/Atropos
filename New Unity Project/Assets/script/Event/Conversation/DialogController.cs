using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 控制对话界面的显示和隐藏以及控制对话进行
/// </summary>
public class DialogController : MonoBehaviour
{
    public EventController eventController;
    public DialogViewer dialogViewer;
    public ConversationScript conversation;
    public string conversationItemKey;
    public static DialogController instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
    /// <summary>
    /// 首次显示对话框使用，如果conversationItemKey为空则默认对话从头开始
    /// </summary>
    /// <param name="conversation"></param>
    /// <param name="conversationItemKey"></param>
    public void Show(ConversationScript conversation, EventController eventController , string conversationItemKey = null)
    {
        this.eventController = eventController;
        this.conversation = conversation;
        ConversationPiece ci;
        if (string.IsNullOrEmpty(conversationItemKey))
            ci = conversation.items[0];
        else
            ci = conversation.Get(conversationItemKey);
        dialogViewer.gameObject.SetActive(true);
        dialogViewer.Show(ci);
    }
    /// <summary>
    /// <para>除首次显示对话框，之后通过此方法根据conversationItemKey将对应对话传入DialogViewer使其更新对话框</para>
    /// <para>如果conversationItemKey没有对应的对话 则结束对话并隐藏对话窗口</para>
    /// </summary>
    /// <param name="conversationItemKey">对话ID</param>
    public void Show(string conversationItemKey)
    {
        if (conversationItemKey == null)
            return;
        ConversationPiece ci;
        ci = conversation.Get(conversationItemKey);
        if (string.IsNullOrEmpty(ci.id) || dialogViewer.IsEnd())
        {
            if(dialogViewer.CP.savePoint)
            {
                PlayerController.instance.SetStep(eventController.ID, dialogViewer.CP.saveTargetID);
            }
            if(dialogViewer.IsHid())
            {
                PlayerController.instance.eventFlage[SceneManager.GetActiveScene().buildIndex - 1][eventController.ID] = false;
            }
            eventController.EndConversation();
            Hid();
            return;
        }
        dialogViewer.gameObject.SetActive(true);
        dialogViewer.Show(ci);
    }
    /// <summary>
    /// 隐藏对话窗口
    /// </summary>
    void Hid()
    {
        dialogViewer.gameObject.SetActive(false);
    }

    /// <summary>
    /// 按左边的按钮或者Q键触发
    /// </summary>
    public void Button0Click()
    {
        if (dialogViewer.IsCode(0))
            eventController.StartCode();
        else
            Show(dialogViewer.GetTarget(0));
    }
    /// <summary>
    /// 按右边的按钮或者E键触发
    /// </summary>
    public void Button1Click()
    {
        if (dialogViewer.IsCode(1))
            eventController.StartCode();
        else
            Show(dialogViewer.GetTarget(1));
    }
}
