using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    Interactable interactable;
    public Animator doorAnimator;
    GameManagerStateMachine gameManager;
    [Header("The number of the room which is being opened)")]
    [Header("0 = None, 1 = Living Room, 2 = Bathroom, 3 = Kitchen")]
    [Header("4 = Bedroom, 5 = Study Room, 6 = Dining Room")]
    public int roomNumber;
    public bool chiefAlt;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManagerStateMachine>();
        interactable= GetComponent<Interactable>();
        interactable.interactType = "Door";
        doorAnimator = GetComponent<Animator>();
    }

    public void DoorPressed()
    {
        Debug.Log(roomNumber);
        Debug.Log("Door Pressed");
    }

    public void StartedAnimation()
    {
        if (roomNumber != 0 || chiefAlt)
        {
            gameManager.fadingScript.fading = true;
        }
    }
    public void FinishedAnimation()
    {
        transform.GetComponent<BoxCollider>().enabled = false;
        if (chiefAlt)
        {
            if (gameManager.inRoom)
            {
                gameManager.roomsSearched.Add(gameManager.currentRoomNumber);
                gameManager.finishedInvestigating = true;
            }
            else if (gameManager.inDeliberation)
            {
                gameManager.finishedInvestigating = false;
                gameManager.stopInteract = true;
            }
        }
        else
        {
            if (roomNumber != 0)
            {
                gameManager.currentRoomNumber = roomNumber;
                gameManager.hasRoomBeenChosen = true;
            }
        }
        gameManager.fadingScript.fading = false;
    }
}
