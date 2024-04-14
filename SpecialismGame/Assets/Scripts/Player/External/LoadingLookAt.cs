using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LoadingLookAt : MonoBehaviour
{
    GameManagerStateMachine gameManager;
    PlayerInteractState playerInteractState;
    PlayerStateMachine playerStateMachine;
    [SerializeField] GameObject Player;
    Camera cameraMain;

    private void Awake()
    {
        cameraMain = Camera.main;
        Player = GameObject.FindWithTag("Player");
        //playerInteractState = 
        playerStateMachine = Player.GetComponent<PlayerStateMachine>();
        gameManager = FindObjectOfType<GameManagerStateMachine>();
    }
    void Update()
    {
        transform.LookAt(cameraMain.transform.position);
        if(gameManager.inCutscene)
        {
            GetComponent<SpriteRenderer>().enabled= false;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled= true;

        }
    }
    void Interact()
    {
        if (!gameManager.inCutscene)
        {
            playerStateMachine.interactedCS1 = true;
            playerStateMachine.loading = false;
        }
        Object.Destroy(gameObject);
    }
}
