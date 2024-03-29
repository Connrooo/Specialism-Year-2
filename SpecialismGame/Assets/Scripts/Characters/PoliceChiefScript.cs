using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceChiefScript : MonoBehaviour
{
    [SerializeField] Vector3[] roomPositions;
    GameManagerStateMachine gameManager;
    Vector3 startingRotation;
    GameObject Player;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        gameManager = FindObjectOfType<GameManagerStateMachine>();
        startingRotation = transform.rotation.eulerAngles;
    }
    private void Update()
    {
        transform.LookAt(Player.transform.position);
        var currentRotation = transform.rotation.eulerAngles;
        currentRotation.x = startingRotation.x;
        currentRotation.z = startingRotation.z;
        transform.rotation = Quaternion.Euler(currentRotation);
        if (gameManager.inRoom)
        {
            transform.position = roomPositions[gameManager.currentRoomNumber-1];
        }
        else if (gameManager.inDeliberation)
        {
            if (gameManager.accusingSuspect || gameManager.day == 3)
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
