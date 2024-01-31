using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class PlayerMotion : MonoBehaviour
{
    PInputManager PInputManager;
    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody pRB;
    [Header("Player Speed")]
    [SerializeField] float movementSpeed;
    

    private void Awake()
    {
        PInputManager = GetComponent<PInputManager>();
        pRB = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }
    public void MovementHandler()
    {
        Move();
    }
    private void Move()
    {
        moveDirection = cameraObject.forward * PInputManager.vertInput;
        moveDirection = moveDirection + cameraObject.right * PInputManager.horInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection = moveDirection * movementSpeed;
        Vector3 movementVelocity = moveDirection;
        pRB.velocity = movementVelocity;
    }
}
