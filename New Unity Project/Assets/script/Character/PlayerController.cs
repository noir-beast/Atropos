using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 相应玩家的控制，控制角色移动，互动，修改角色的动画
/// </summary>
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public string[][] eventStepKey;
    public bool[][] eventFlage;
    public bool inEvent = false;
    public bool inCoding = false;
    public EventController eventController;
    public float speed = 25;
    public float acceleration = 0.1f;
    public float stepSize = 0.1f;
    public Animator animator;

    public Vector2 lookDirection = new Vector2(0,-1);
    public Vector2 currentVelocity;
    public Vector3 nextMoveCommand;
    //float velocity;

    Rigidbody2D rbody;
    private void Awake()
    {
        if (DataKeeper.instance)
        {
            instance = this;
            transform.position = DataKeeper.instance.playerPosition;
            eventStepKey = DataKeeper.instance.eventStepKey;
            eventFlage = DataKeeper.instance.eventFlage;
            lookDirection = DataKeeper.instance.lookDirection;
            rbody = GetComponent<Rigidbody2D>();
            return;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CharacterControl();
        //velocity = Mathf.Clamp01(velocity + Time.deltaTime * acceleration);
        UpdateAnimator();
        rbody.velocity = Vector2.SmoothDamp(rbody.velocity, nextMoveCommand * speed, ref currentVelocity, acceleration, speed);
    }

    void UpdateAnimator()
    {
        if (animator)
        {
            animator.SetFloat("Speed", nextMoveCommand.magnitude*10);
            animator.SetFloat("LookX", lookDirection.x);
            animator.SetFloat("LookY", lookDirection.y);
        }
    }

    void CharacterControl()
    {
        if (inCoding)
            return;
        if(!inCoding && !inEvent)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DataKeeper.instance.KeepData(this);
                SceneManager.LoadScene("menu");

            }
        }
        if(inEvent == false)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                nextMoveCommand = Vector3.up * stepSize;
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
                nextMoveCommand = Vector3.down * stepSize;
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                nextMoveCommand = Vector3.left * stepSize;
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                nextMoveCommand = Vector3.right * stepSize;
            else
                nextMoveCommand = Vector3.zero;
            if(nextMoveCommand != Vector3.zero) lookDirection = nextMoveCommand / stepSize;
        }
        else
            nextMoveCommand = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (inEvent == false)
            {
                RaycastHit2D hit = Physics2D.Raycast(rbody.position, lookDirection, 1f, LayerMask.GetMask("Water"));
                if (hit.collider != null)
                {
                    inEvent = true;
                    eventController = hit.collider.GetComponent<EventController>();
                    if (eventController != null)
                        eventController.StartConversation(this);
                    else
                        inEvent = false; ;
                }
            }
            else
            {
                DialogController.instance.Button0Click();
            }
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            if (inEvent)
            {
                DialogController.instance.Button1Click();
            }
        }
    }

    public void SetinEvent(bool b)
    {
        inEvent = b;
    }
    public string GetStep(int eventID)
    {
        if (eventStepKey[SceneManager.GetActiveScene().buildIndex -1].Length >= eventID)
            return eventStepKey[SceneManager.GetActiveScene().buildIndex -1][eventID];
        else
            return null;
    }

    public void SetStep(int eventID, string step)
    {
        if (eventStepKey[SceneManager.GetActiveScene().buildIndex - 1].Length >= eventID)
        {
            eventStepKey[SceneManager.GetActiveScene().buildIndex - 1][eventID] = step;
        }
    }
}
