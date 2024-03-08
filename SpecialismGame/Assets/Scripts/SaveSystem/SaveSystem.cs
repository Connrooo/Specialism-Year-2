using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveGameplay(GameManagerStateMachine gameManager)
    {
        string saveNumber = gameManager.saveNumber.ToString();  
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/gamestate.saveddata"+saveNumber;
        FileStream stream = new FileStream(path, FileMode.Create);

        GameplayData data = new GameplayData(gameManager);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameplayData LoadGameplay(GameManagerStateMachine gameManager)
    {
        string saveNumber = gameManager.saveNumber.ToString();
        string path = Application.persistentDataPath + "/gamestate.saveddata" + saveNumber;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameplayData data = formatter.Deserialize(stream) as GameplayData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
