using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class PlayerMotion : MonoBehaviour
{
    PInputManager PInputManager;
    ControlSchemeState controlSchemeState;
    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody pRB;
    [Header("Player Speed")]
    [SerializeField] float movementSpeed;
    

    private void Awake()
    {
        PInputManager = GetComponent<PInputManager>();
        pRB = GetComponent<Rigidbody>();
        controlSchemeState = FindObjectOfType<ControlSchemeState>();
        cameraObject = Camera.main.transform;
    }
    public void MovementHandler()
    {
        Move();
    }
    private void Move()
    {
        switch (controlSchemeState.controlScheme)
        {
            case 0:
                DefaultControls();
                break;
            case 1:
                DefaultControls();
                break;
            case 2:
                MouseOnly();
                break;
        }
        moveDirection.y = 0;
        moveDirection = moveDirection * movementSpeed;
        Vector3 movementVelocity = moveDirection;
        pRB.velocity = movementVelocity;
    }
    private void DefaultControls()
    {
        moveDirection = cameraObject.forward * PInputManager.vertInput;
        moveDirection = moveDirection + cameraObject.right * PInputManager.horInput;
        moveDirection.Normalize();
    }
    private void MouseOnly()
    {
        moveDirection = Vector3.zero;
        if (PInputManager.interactInput)
        {
            moveDirection = cameraObject.forward;
        }
        if(PInputManager.backwardsInput)
        {
            moveDirection = -cameraObject.forward;
        }
        if (PInputManager.interactInput&& PInputManager.backwardsInput)
        {
            moveDirection = -cameraObject.forward*0;
        }
        moveDirection = moveDirection + cameraObject.right * 0;

    }
}
