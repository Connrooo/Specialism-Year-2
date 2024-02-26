using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCamera : MonoBehaviour
{
    GameManagerStateMachine gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManagerStateMachine>();
        Debug.Log(gameManager.enabled);
    }

    private void animationOver()
    {
        gameManager.stopAnimation = true;
        Debug.Log("Sent");
    }
}
