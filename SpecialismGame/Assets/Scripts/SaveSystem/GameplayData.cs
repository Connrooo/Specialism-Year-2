using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameplayData
{
    public int currentRoomNumber;
    public int suspectAccused;
    public int day;
    public bool hasRoomBeenChosen;
    public bool finishedInvestigating;
    public float[] playerPosition;
    public List<int> roomsSearched;
    //public List<string> pickedUpObjectsName;
    //public List<string> pickedUpObjectsDescription;
    //public List<int> pickedUpObjectsSuspect;
    public List<PickupObjects> pickupObjects;

    public GameplayData(GameManagerStateMachine gameManager) 
    {
        currentRoomNumber = gameManager.currentRoomNumber;
        suspectAccused = gameManager.suspectAccused;
        day = gameManager.day;
        hasRoomBeenChosen = gameManager.hasRoomBeenChosen;
        finishedInvestigating = gameManager.finishedInvestigating;
        playerPosition = new float[3];
        playerPosition[0] = gameManager.Player.transform.position.x;
        playerPosition[1] = gameManager.Player.transform.position.y;
        playerPosition[2] = gameManager.Player.transform.position.z;
        roomsSearched = gameManager.roomsSearched;
        foreach (CluePickup clue in gameManager.pickedUpObjects)
        {
            PickupObjects objectPickup = new PickupObjects();
            objectPickup.pickedUpObjectsName = clue.itemName;
            objectPickup.pickedUpObjectsDescription = clue.itemDescription;
            objectPickup.pickedUpObjectsSuspect = clue.suspectRelated;
            //pickupObjects.pickedUpObjectsName.Add(clue.itemName);
            //pickedUpObjectsDescription.Add(clue.itemDescription);
            //pickedUpObjectsSuspect.Add(clue.suspectRelated);

        }
    }
}

public class PickupObjects
{
    public string pickedUpObjectsName;
    public string pickedUpObjectsDescription;
    public int pickedUpObjectsSuspect;
}
