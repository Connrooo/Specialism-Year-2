using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PInputManager PInputManager;
    ControlSchemeState controlSchemeState;
    Vector3 moveDirection;
    Transform cameraObject;
    [Header("Player Speed")]
    [SerializeField] float movementSpeed;

    private void Awake()
    {
        PInputManager = GetComponent<PInputManager>();
        controlSchemeState = FindObjectOfType<ControlSchemeState>();
        cameraObject = Camera.main.transform;
    }
    public void PlayerHandler()
    {

    }
}