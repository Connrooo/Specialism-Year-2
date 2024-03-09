using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SaveLoadScript : MonoBehaviour
{
    GameManagerStateMachine gameManager;
    MenuManager menuManager;
    [SerializeField] TMP_Text[] saveButtonDayText;
    [SerializeField] TMP_Text[] saveButtonRoomText;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManagerStateMachine>();
        menuManager = FindObjectOfType<MenuManager>();
    }

    private void OnApplicationQuit()
    {
        if (gameManager.playingGame)
        {
            SaveGameplay();
        }
    }

    private void Start()
    {
        UpdateLevelDetails();
    }

    public void SaveGameplay()
    {
        SaveSystem.SaveGameplay(gameManager);
        ChangeSaveButtonValues();
    }

    private void ChangeSaveButtonValues()
    {
        switch(gameManager.saveNumber)
        {
            case 1:
                PlayerPrefs.SetString("S1Day", "Day " + gameManager.day);
                PlayerPrefs.SetString("S1Room", FindRoom());
                break;
            case 2:
                PlayerPrefs.SetString("S2Day", "Day " + gameManager.day);
                PlayerPrefs.SetString("S2Room", FindRoom());
                break;
            case 3:
                PlayerPrefs.SetString("S3Day", "Day " + gameManager.day);
                PlayerPrefs.SetString("S3Room", FindRoom());
                break;
        }
        UpdateLevelDetails();
    }

    private string FindRoom()
    {
        switch(gameManager.roomDisplayValue)
        {
            case 0:
                return "Hallway";
            case 1:
                switch(gameManager.currentRoomNumber)
                {
                    case 1:
                        return "Living Room";
                    case 2:
                        return "Bathroom";
                    case 3:
                        return "Kitchen";
                    case 4:
                        return "Bedroom";
                    case 5:
                        return "Study Room";
                    case 6:
                        return "Dining Room";
                }
                break;
            case 2:
                return "Deliberation";
            case 3:
                return "Closing";
        }
        return "";
    }

    private void UpdateLevelDetails()
    {
        saveButtonDayText[0].text = PlayerPrefs.GetString("S1Day", "New Save");
        saveButtonDayText[1].text = PlayerPrefs.GetString("S2Day", "New Save");
        saveButtonDayText[2].text = PlayerPrefs.GetString("S3Day", "New Save");
        saveButtonRoomText[0].text = PlayerPrefs.GetString("S1Room", "");
        saveButtonRoomText[1].text = PlayerPrefs.GetString("S2Room", "");
        saveButtonRoomText[2].text = PlayerPrefs.GetString("S3Room", "");
    }


    public void LoadGameplay()
    {
        GameplayData data = SaveSystem.LoadGameplay(gameManager);
        gameManager.currentRoomNumber = data.currentRoomNumber;
        gameManager.suspectAccused = data.suspectAccused;
        gameManager.day = data.day;
        gameManager.hasRoomBeenChosen = data.hasRoomBeenChosen;
        gameManager.finishedInvestigating = data.finishedInvestigating;
        Vector3 playerPosition = new Vector3(data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]);
        gameManager.Player.transform.position = playerPosition;
        gameManager.roomsSearched = data.roomsSearched;
        for(int i = 0; i < data.pickedUpObjectsName.Count; i++)
        {
            CluePickup clue = new();
            clue.itemName = data.pickedUpObjectsName[i];
            clue.itemDescription = data.pickedUpObjectsDescription[i];
            clue.suspectRelated = data.pickedUpObjectsSuspect[i];
            gameManager.pickedUpObjects.Add(clue);
        }
    }
    public void Save()
    {
        SaveGameplay();
    }

    public void NewGame(int saveNumber)
    {
        gameManager.pickedUpObjects = new();
        gameManager.currentRoomNumber = 0;
        gameManager.suspectAccused = 0;
        gameManager.day = 1;
        gameManager.hasRoomBeenChosen = false;
        gameManager.finishedInvestigating = false;
        gameManager.Player.transform.position = new Vector3(0, 1, 0);
        gameManager.roomsSearched = new List<int> { };
        gameManager.saveNumber = saveNumber;
        menuManager.StartGame();
    }

    public void LoadGame(int saveNumber)
    {
        gameManager.pickedUpObjects = new();
        gameManager.saveNumber = saveNumber;
        LoadGameplay();
        menuManager.StartGame();
    }
}
