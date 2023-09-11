using System;

[Serializable]
public class SaveData
{
    public string sceneName;
    public float playerPositionX;
    public float playerPositionY;
    public float playerPositionZ;
    public string[][] eventStepKey;
    public bool[][] eventFlage;
    public float lookDirectionX;
    public float lookDirectionY;

    public SaveData()
    {

    }

    public SaveData(DataKeeper dateKeeper)
    {
        sceneName = dateKeeper.sceneName;
        playerPositionX = dateKeeper.playerPosition.x;
        playerPositionY = dateKeeper.playerPosition.y;
        playerPositionZ = dateKeeper.playerPosition.z;
        eventStepKey = dateKeeper.eventStepKey;
        eventFlage = dateKeeper.eventFlage;
        lookDirectionX = dateKeeper.lookDirection.x;
        lookDirectionY = dateKeeper.lookDirection.y;
    }
}
