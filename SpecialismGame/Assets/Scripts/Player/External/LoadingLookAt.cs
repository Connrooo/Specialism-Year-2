using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LoadingLookAt : MonoBehaviour
{
    PlayerStateMachine playerStateMachine;
    [SerializeField] GameObject Player;
    Camera cameraMain;

    private void Awake()
    {
        cameraMain = Camera.main;
        Player = GameObject.FindWithTag("Player");
        playerStateMachine = Player.GetComponent<PlayerStateMachine>();
    }
    void Update()
    {
        transform.LookAt(cameraMain.transform.position);
    }
    void Kill()
    {
        playerStateMachine.loading = false;
        Destroy(playerStateMachine.currentObject);
        Destroy(gameObject);
    }
}
