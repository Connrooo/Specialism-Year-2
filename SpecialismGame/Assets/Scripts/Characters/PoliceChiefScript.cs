using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceChiefScript : MonoBehaviour
{
    [SerializeField] Vector3[] roomPositions;
    GameManagerStateMachine gameManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManagerStateMachine>();
    }
    private void Update()
    {
        if (gameManager.inRoom)
        {
            transform.position = roomPositions[gameManager.currentRoomNumber-1];
        }
        else if (gameManager.inDeliberation)
        {
            if (gameManager.accusingSuspect)
            {
                transform.position = new Vector3(0, 0.025f, 3.5f);
            }
            else
            {
                transform.position = new Vector3(0, 0.025f, -1.5f);
            }
        }
        else
        {
            transform.position = new Vector3(100, 0.025f, 100);
        }
    }
}
