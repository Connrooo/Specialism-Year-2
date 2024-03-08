using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SaveLoadScript : MonoBehaviour
{
    GameManagerStateMachine gameManager;
    TMP_Text[] saveButtonDayText;
    TMP_Text[] saveButtonRoomText;


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManagerStateMachine>();
    }

    public void SaveGameplay()
    {
        SaveSystem.SaveGameplay(gameManager);
    }
    public void LoadGameplay()
    {
        GameplayData data = SaveSystem.LoadGameplay(gameManager);
        gameManager.currentRoomNumber = data.currentRoomNumber;
        gameManager.suspectAccused = data.suspectAccused;
        gameManager.day = data.day;
        gameManager.hasRoomBeenChosen = data.hasRoomBeenChosen;
        gameManager.finishedInvestigating = data.finishedInvestigating;
        Vector3 playerPosition = new Vector3(data.playerPosition[0], data.playerPosition[1], data.playerPosition[3]);
        gameManager.Player.transform.position = playerPosition;
        gameManager.roomsSearched = data.roomsSearched;
        foreach (PickupObjects clueObject in data.pickupObjects)
        {
            CluePickup clue = new CluePickup();
            clue.itemName = clueObject.pickedUpObjectsName;
            clue.itemDescription = clueObject.pickedUpObjectsDescription;
            clue.suspectRelated = clueObject.pickedUpObjectsSuspect;
            gameManager.pickedUpObjects.Add(clue);
        }
    }
    public void Save()
    {
        SaveGameplay();
    }

    public void NewGame()
    {
        gameManager.currentRoomNumber = 0;
        gameManager.suspectAccused = 0;
        gameManager.day = 1;
        gameManager.hasRoomBeenChosen = false;
        gameManager.finishedInvestigating = false;
        gameManager.Player.transform.position = new Vector3(0, 1, 0);
        gameManager.roomsSearched = new List<int> { };
    }

    public void Load(int saveNumber)
    {
        gameManager.saveNumber = saveNumber;
        LoadGameplay();
    }
}
