using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform focus;
    public float smoothTime = 2;
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = focus.position - transform.position;
        offset.x = 0;
        offset.y = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, focus.position - offset, Time.deltaTime * smoothTime);
        //transform.position = focus.position - offset;
    }
}
