using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LoadingLookAt : MonoBehaviour
{
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
    }
    void Update()
    {
        transform.LookAt(cameraMain.transform.position);
    }
    void Interact()
    {
        playerStateMachine.loading = false;
        playerStateMachine.interactedCS1 = true;
        Object.Destroy(gameObject);
    }
}
