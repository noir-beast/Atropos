using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    public string target;
    public Vector3 targetPosition;
    public Vector3 lookDirection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerController player = collider.GetComponent<PlayerController>();
        if (player != null)
        {
            DataKeeper.instance.KeepData(player);
            DataKeeper.instance.playerPosition = targetPosition;
            DataKeeper.instance.lookDirection = lookDirection;
            SceneManager.LoadScene(target);
        }
    }
}
