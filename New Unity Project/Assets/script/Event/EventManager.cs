using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 统一事件管理
/// </summary>
public class EventManager : MonoBehaviour
{
    public EventController[] events;
    // Start is called before the first frame update
    void Start()
    {
        events = new EventController[transform.childCount];
        for(int i = 0; i<transform.childCount; i++)
        {
            events[i] = transform.GetChild(i).GetComponent<EventController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEvent();
    }

    public void UpdateEvent()
    {
        bool[] temp = PlayerController.instance.eventFlage[SceneManager.GetActiveScene().buildIndex -1];

        for(int i = 0; i < events.Length;i++)
        {
            events[i].gameObject.SetActive(temp[events[i].ID]);
        }


    }
}
