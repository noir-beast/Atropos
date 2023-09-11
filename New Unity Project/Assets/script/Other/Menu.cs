using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Button s;
    public Button e;
    public Button d;
    public float angle;
    // Update is called once per frame


    void Update()
    {
        transform.RotateAround(transform.position, Vector3.back, angle);
        s.transform.RotateAround(s.transform.position, Vector3.back, -angle);
        e.transform.RotateAround(e.transform.position, Vector3.back, -angle);
        d.transform.RotateAround(d.transform.position, Vector3.back, -angle);
    }
    public void StartButton()
    {
        DataKeeper.instance.InitData();
        SceneManager.LoadScene("0");

    }
    public void ContinueButton()
    {
        if (DataKeeper.instance.sceneName != null && DataKeeper.instance.sceneName != "")
        {
            SceneManager.LoadScene(DataKeeper.instance.sceneName);
        }
        if (!FileIO.IsDirectoryExists(Application.persistentDataPath + "/Save") || !FileIO.IsFileExists(Application.persistentDataPath + "/Save/save.sav"))
            return;
        SaveData date = new SaveData();
        date = (SaveData)FileIO.GetData(Application.persistentDataPath + "/Save/save.sav", date.GetType());
        if (date == null ||date.sceneName == null || date.sceneName == "")
            return;
        DataKeeper.instance.KeepData(date);
        SceneManager.LoadScene(DataKeeper.instance.sceneName);

    }
    public void ExitButton()
    {
        if(!string.IsNullOrEmpty(DataKeeper.instance.sceneName))
        {
            if(!FileIO.IsDirectoryExists(Application.persistentDataPath +"/Save"))
            {
                FileIO.CreateDirectory(Application.persistentDataPath + "/Save");
            }
            if(FileIO.IsFileExists(Application.persistentDataPath + "/Save/save.sav"))
            {
                FileIO.CreateFile(Application.persistentDataPath + "/Save/save.sav", "");
            }
            SaveData date = new SaveData(DataKeeper.instance);
            FileIO.SetData(Application.persistentDataPath + "/Save/save.sav", date);
        }

        Application.Quit();
    }

}
